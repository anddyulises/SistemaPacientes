(function () {
  'use strict';
  var API = (window.API_URL || '').replace(/\/$/, '');
  var S = window.SCHEMA;
  var byRoute = {};
  S.forEach(function (e) { byRoute[e.route] = e; });
  var $ = function (s) { return document.querySelector(s); };
  var cache = {};          // route -> arreglo de registros
  var current = null;      // entidad mostrada
  var editingId = null;
  var deletingId = null;

  function esc(s) { return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); }

  /* ---------- Acceso al API ---------- */
  async function call(method, path, body) {
    var res;
    try {
      res = await fetch(API + path, {
        method: method,
        headers: body ? { 'Content-Type': 'application/json' } : undefined,
        body: body ? JSON.stringify(body) : undefined
      });
    } catch (e) {
      setApi(false);
      return { success: false, message: 'No se pudo conectar con el API (' + API + '). ¿Está corriendo?' };
    }
    setApi(true);
    try { return await res.json(); }
    catch (e) { return { success: false, message: 'Respuesta inesperada del servidor (' + res.status + ').' }; }
  }
  function setApi(ok) {
    var el = $('#api-state');
    el.textContent = ok ? 'API conectado' : 'API sin conexión';
    el.className = 'api-state' + (ok ? '' : ' bad');
  }
  async function load(route) {
    var r = await call('GET', '/' + route);
    cache[route] = r.success ? r.data : [];
    if (!r.success) toast(r.message, true);
    return cache[route];
  }

  /* ---------- Utilidades de presentación ---------- */
  function labelOf(entity, row) {
    if (!row) return '';
    return entity.labelfmt.replace(/\{(\w+)\}/g, function (_, k) { return row[k] == null ? '' : row[k]; });
  }
  function fkLabel(route, id) {
    if (id == null) return '—';
    var row = (cache[route] || []).filter(function (r) { return r.id === id; })[0];
    return row ? labelOf(byRoute[route], row) : '#' + id;
  }
  function fmt(f, v) {
    if (v == null || v === '') return '—';
    if (f.t === 'fk') return esc(fkLabel(f.fk, v));
    if (f.t === 'date') return esc(new Date(v).toLocaleDateString('es-DO'));
    if (f.t === 'datetime') return esc(new Date(v).toLocaleString('es-DO', { dateStyle: 'short', timeStyle: 'short' }));
    if (f.t === 'dec') return esc(Number(v).toLocaleString('es-DO', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
    return esc(v);
  }
  var toastTimer;
  function toast(msg, bad) {
    var t = $('#toast'); t.textContent = msg; t.className = 'toast' + (bad ? ' bad' : ''); t.hidden = false;
    clearTimeout(toastTimer); toastTimer = setTimeout(function () { t.hidden = true; }, 4500);
  }

  /* ---------- Navegación ---------- */
  function buildNav() {
    $('#nav').innerHTML = '<a href="#inicio" data-r="inicio">Inicio</a>' +
      S.map(function (e) { return '<a href="#' + e.route + '" data-r="' + e.route + '">' + esc(e.label) + '</a>'; }).join('');
  }
  async function route() {
    var r = location.hash.slice(1) || 'inicio';
    var ent = byRoute[r];
    document.querySelectorAll('#nav a').forEach(function (a) {
      if (a.getAttribute('data-r') === r || (!ent && a.getAttribute('data-r') === 'inicio' && r === 'inicio')) a.setAttribute('aria-current', 'page'); else a.removeAttribute('aria-current');
    });
    $('#search').value = '';
    if (!ent) { current = null; await renderHome(); return; }
    current = ent;
    $('#title').textContent = ent.label;
    $('#btn-new').hidden = false; $('#search').hidden = false;
    $('#view').innerHTML = '<p class="lead">Cargando…</p>';
    var needed = [ent.route].concat(ent.fields.filter(function (f) { return f.fk; }).map(function (f) { return f.fk; }));
    await Promise.all(needed.filter(function (v, i, a) { return a.indexOf(v) === i; }).map(load));
    renderTable();
  }
  async function renderHome() {
    $('#title').textContent = 'Inicio';
    $('#btn-new').hidden = true; $('#search').hidden = true;
    $('#view').innerHTML = '<p class="lead">Gestione pacientes, médicos, citas, consultas y recetas. Elija una tabla del menú o entre desde aquí.</p><div class="cards" id="cards"></div>';
    await Promise.all(S.map(function (e) { return load(e.route); }));
    $('#cards').innerHTML = S.map(function (e) {
      return '<a class="card" href="#' + e.route + '"><b>' + (cache[e.route] || []).length + '</b><span>' + esc(e.label) + '</span></a>';
    }).join('');
  }
  function renderTable() {
    var ent = current, q = $('#search').value.trim().toLowerCase();
    var rows = (cache[ent.route] || []).filter(function (r) {
      if (!q) return true;
      return ent.fields.some(function (f) {
        var txt = f.t === 'fk' ? fkLabel(f.fk, r[f.n]) : r[f.n];
        return String(txt == null ? '' : txt).toLowerCase().indexOf(q) >= 0;
      });
    });
    if (!rows.length) {
      $('#view').innerHTML = '<div class="tablewrap"><div class="empty">' + (q ? 'Sin resultados para la búsqueda.' : 'No hay registros todavía. Use “Nuevo registro”.') + '</div></div>';
      return;
    }
    var head = '<th>#</th>' + ent.fields.map(function (f) { return '<th>' + esc(f.label) + '</th>'; }).join('') + '<th></th>';
    var body = rows.map(function (r) {
      return '<tr><td class="num">' + r.id + '</td>' + ent.fields.map(function (f) {
        return '<td' + (f.t === 'dec' || f.t === 'int' ? ' class="num"' : '') + '>' + fmt(f, r[f.n]) + '</td>';
      }).join('') + '<td class="acts"><button class="btn sm" data-edit="' + r.id + '" type="button">Editar</button><button class="btn sm" data-del="' + r.id + '" type="button">Eliminar</button></td></tr>';
    }).join('');
    $('#view').innerHTML = '<div class="tablewrap"><table><thead><tr>' + head + '</tr></thead><tbody>' + body + '</tbody></table></div>';
  }

  /* ---------- Formulario ---------- */
  function inputFor(f, value) {
    var id = 'f-' + f.n, v = value;
    var base = ' id="' + id + '" name="' + f.n + '"';
    if (f.t === 'choice') {
      return '<select' + base + '>' + f.choices.map(function (c) { return '<option' + (v === c ? ' selected' : '') + '>' + esc(c) + '</option>'; }).join('') + '</select>';
    }
    if (f.t === 'fk') {
      var opts = (cache[f.fk] || []).map(function (r) { return '<option value="' + r.id + '"' + (v === r.id ? ' selected' : '') + '>' + esc(labelOf(byRoute[f.fk], r)) + '</option>'; }).join('');
      return '<select' + base + '><option value="">' + (f.req ? '— Seleccione —' : '— Ninguno —') + '</option>' + opts + '</select>';
    }
    if (f.t === 'int') return '<input' + base + ' type="number" step="1" value="' + esc(v == null ? '' : v) + '">';
    if (f.t === 'dec') return '<input' + base + ' type="number" step="0.01" value="' + esc(v == null ? '' : v) + '">';
    if (f.t === 'date') return '<input' + base + ' type="date" value="' + esc(v ? String(v).slice(0, 10) : '') + '">';
    if (f.t === 'datetime') return '<input' + base + ' type="datetime-local" value="' + esc(v ? String(v).slice(0, 16) : '') + '">';
    return '<input' + base + ' type="text"' + (f.max ? ' maxlength="' + (f.max + 50) + '"' : '') + ' value="' + esc(v == null ? '' : v) + '" autocomplete="off">';
  }
  function openForm(id) {
    var ent = current; editingId = id;
    var row = id ? (cache[ent.route] || []).filter(function (r) { return r.id === id; })[0] : null;
    $('#dlg-title').textContent = (id ? 'Editar · ' : 'Nuevo · ') + ent.label;
    $('#form-error').hidden = true;
    $('#fields').innerHTML = ent.fields.map(function (f) {
      var full = f.t === 'str' && (f.max || 0) >= 150 ? ' full' : '';
      return '<div class="field' + full + '"><label for="f-' + f.n + '">' + esc(f.label) + (f.req ? ' <span class="req" aria-hidden="true">*</span>' : '') + '</label>' + inputFor(f, row ? row[f.n] : (f.t === 'choice' ? f.choices[0] : null)) + '</div>';
    }).join('');
    $('#dlg-form').showModal();
    var first = $('#fields input, #fields select'); if (first) first.focus();
  }
  function collect() {
    var body = {};
    current.fields.forEach(function (f) {
      var raw = $('#f-' + f.n).value;
      if (f.t === 'str') body[f.n] = raw;
      else if (f.t === 'choice') body[f.n] = raw;
      else if (f.t === 'int') body[f.n] = raw === '' ? 0 : parseInt(raw, 10);
      else if (f.t === 'dec') body[f.n] = raw === '' ? 0 : parseFloat(raw);
      else if (f.t === 'date') body[f.n] = raw === '' ? '0001-01-01T00:00:00' : raw + 'T00:00:00';
      else if (f.t === 'datetime') body[f.n] = raw === '' ? '0001-01-01T00:00:00' : raw + ':00';
      else if (f.t === 'fk') body[f.n] = raw === '' ? (f.req ? 0 : null) : parseInt(raw, 10);
    });
    return body;
  }
  async function submit(ev) {
    ev.preventDefault();
    var body = collect(), r;
    if (editingId) { body.id = editingId; r = await call('PUT', '/' + current.route + '/' + editingId, body); }
    else r = await call('POST', '/' + current.route, body);
    if (!r.success) { var b = $('#form-error'); b.textContent = r.message; b.hidden = false; return; }
    $('#dlg-form').close();
    toast(r.message);
    await load(current.route);
    renderTable();
  }

  /* ---------- Eliminar ---------- */
  function askDelete(id) {
    deletingId = id;
    var row = (cache[current.route] || []).filter(function (r) { return r.id === id; })[0];
    $('#del-text').textContent = 'Se eliminará: ' + labelOf(current, row) + '.';
    $('#dlg-del').showModal();
  }
  async function doDelete() {
    var r = await call('DELETE', '/' + current.route + '/' + deletingId);
    $('#dlg-del').close();
    toast(r.message, !r.success);
    await load(current.route);
    renderTable();
  }

  /* ---------- Eventos ---------- */
  document.addEventListener('click', function (ev) {
    var t = ev.target.closest('button'); if (!t) return;
    if (t.id === 'btn-new') openForm(null);
    else if (t.id === 'btn-cancel') $('#dlg-form').close();
    else if (t.id === 'del-no') $('#dlg-del').close();
    else if (t.id === 'del-yes') doDelete();
    else if (t.hasAttribute('data-edit')) openForm(Number(t.getAttribute('data-edit')));
    else if (t.hasAttribute('data-del')) askDelete(Number(t.getAttribute('data-del')));
  });
  $('#form').addEventListener('submit', submit);
  $('#search').addEventListener('input', function () { if (current) renderTable(); });
  window.addEventListener('hashchange', route);
  buildNav(); route();
})();
