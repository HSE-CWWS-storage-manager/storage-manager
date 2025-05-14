! function(c, p) {
   var t, d = {
      decontextUrl: null,
      popupContainerEl: null,
      resizeTimeout: null,
      updateInterval: 100
   };

   function s(t) {
      t.style.color = "#666", t.style.marginLeft = "15px"
   }

   function u() {
      d.popupContainerEl.style.display = "none", d.popupContainerEl
         .querySelector('[data-decontext-role="popup-body"]')
         .setAttribute("src", ""), d.popupContainerEl.querySelector(
            '[data-decontext-role="popup-open-control"]')
         .setAttribute("href", "javascript:void(0);")
   }

   function o() {
      var t, e = l(),
         o = i(),
         n = null,
         r = null;
      switch ((t = l()) < 768 ? "xs" : t < 992 ? "sm" : t < 1200 ?
         "md" : "lg") {
         case "xs":
            n = Math.floor(.9 * e), r = Math.floor(.9 * o);
            break;
         case "sm":
            n = Math.floor(.8 * e), r = Math.floor(.8 * o);
            break;
         case "md":
            n = Math.floor(.75 * e), r = Math.floor(.75 * o);
            break;
         case "lg":
            n = Math.floor(.65 * e), r = Math.floor(.75 * o)
      }
      return {
         width: n,
         height: r
      }
   }

   function l() {
      return p.innerWidth || c.documentElement.clientWidth || c
         .getElementsByTagName("body")[0].clientWidth
   }

   function i() {
      return p.innerHeight || c.documentElement.clientHeight || c
         .getElementsByTagName("body")[0].clientHeight
   }

   function h() {
      var t = o(),
         e = d.popupContainerEl.querySelector(
            '[data-decontext-role="popup-modal"]');
      e.style.width = t.width.toString() + "px", e.style.height = t
         .height.toString() + "px", e.style.marginTop = ((i() - t
            .height) / 2)
         .toString() + "px", d.popupContainerEl.querySelector(
            '[data-decontext-role="popup-body"]')
         .style.height = (t.height - 30 - 20 - 20)
         .toString() + "px"
   }

   function g(t) {
      var e, o, n, r, l, i;
      d.popupContainerEl.querySelector(
            '[data-decontext-role="popup-body"]')
         .setAttribute("src", (e = t.split("."), o = e.pop(), n = e
            .join("."), d.decontextUrl + "/" + n + "_print." + o)), d
         .popupContainerEl.querySelector(
            '[data-decontext-role="popup-open-control"]')
         .setAttribute("href", (r = t.split("."), l = r.pop(), i = r
            .join("."), d.decontextUrl + "/" + i + "." + l)), h(), d
         .popupContainerEl.style.display = "block"
   }

   function f() {
      return "block" === d.popupContainerEl.style.display
   }

   function y(t, e, o) {
      var n = t.getBoundingClientRect(),
         r = null;
      switch (o.positionX) {
         case "right":
            r = Math.floor(n.right + p.scrollX - o.width / 2)
               .toString() + "px";
            break;
         case "left":
            r = Math.floor(n.left + p.scrollX - o.width / 2)
               .toString() + "px";
            break;
         case "center":
            r = Math.floor((n.right - n.left) / 2 + n.left + p
                  .scrollX - o.width / 2)
               .toString() + "px";
            break;
         default:
            r = "0"
      }
      var l = null;
      switch (o.positionY) {
         case "top":
            l = Math.floor(n.top + p.scrollY - o.height / 2)
               .toString() + "px";
            break;
         case "bottom":
            l = Math.floor(n.bottom + p.scrollY - o.height / 2)
               .toString() + "px";
            break;
         case "center":
            l = Math.floor((n.bottom - n.top) / 2 + n.top + p
                  .scrollY - o.height / 2)
               .toString() + "px";
            break;
         default:
            l = "0"
      }
      r !== e.style.left && (e.style.left = r), l !== e.style.top && (
         e.style.top = l)
   }

   function a(t) {
      var e, o = c.createElement("div");
      (e = o)
      .style.position = "absolute", e.style.boxSizing = "border-box",
         e.style.textAlign = "center", e.style.cursor = "pointer", e
         .style.borderRadius = "50%", e.style.zIndex = 99998, e.style
         .fontSize = "0.9em", e.style.fontFamily =
         "Arial, Helvetica, sans-serif", e.style.fontWeight = "bold",
         o.className = "decontext-callout", o.innerHTML = "?", c
         .querySelector("body")
         .appendChild(o);
      var n = {
            fgColor: "#fff",
            bgColor: "#bbb",
            width: 24,
            height: 24,
            positionX: "right",
            positionY: "top"
         },
         r = t.getAttribute("data-decontext-bgcolor");
      null !== r && (n.bgColor = r);
      var l = t.getAttribute("data-decontext-fgcolor");
      null !== l && (n.fgColor = l);
      var i = t.getAttribute("data-decontext-width");
      null !== i && (n.width = parseInt(i, 10));
      var a = t.getAttribute("data-decontext-height");
      null !== a && (n.height = parseInt(a, 10));
      var p = t.getAttribute("data-decontext-position-x");
      null !== p && (n.positionX = p);
      var d = t.getAttribute("data-decontext-position-y");
      null !== d && (n.positionY = d), o.style.width = n.width
         .toString() + "px", o.style.height = n.height.toString() +
         "px", o.style.lineHeight = n.height.toString() + "px", o
         .style.color = n.fgColor, o.style.backgroundColor = n
         .bgColor, y(t, o, n);
      var s, u = t.getAttribute("data-decontext-filename");
      return null !== u && o.addEventListener("click", (s = u,
         function(t) {
            g(s)
         })), {
         el: t,
         calloutEl: o,
         options: n
      }
   }
   t = function() {
      d.decontextUrl = function() {
            for (var t = c.querySelectorAll("script"), e = 0; e < t
               .length; ++e) {
               var o = t[e].getAttribute("data-decontext-url");
               if (null !== o) return o
            }
            return null
         }(),
         function() {
            var t, e = c.createElement("div");
            e.setAttribute("data-decontext-role",
                  "popup-container"), (t = e)
               .style.zIndex = 99999, t.style.position = "fixed", t
               .style.overflow = "hidden", t.style.top = "0", t
               .style.bottom = "0", t.style.left = "0", t.style
               .right = "0", t.style.backgroundColor =
               "rgba(0, 0, 0, 0.5)";
            var o, n = c.createElement("div");
            n.setAttribute("data-decontext-role", "popup-modal"), (
                  o = n)
               .style.marginLeft = "auto", o.style.marginRight =
               "auto", o.style.marginBottom = "0", o.style
               .backgroundColor = "#fff", o.style.padding =
               "20px 10px 20px 20px", o.style.border =
               "2px solid rgb(102, 102, 102)", o.style
               .borderRadius = "5px", o.style.boxSizing =
               "border-box", e.appendChild(n);
            var r = c.createElement("div");
            r.setAttribute("data-decontext-role", "popup-header"),
               r.style.textAlign = "right", r.style.height =
               "30px", r.style.boxSizing = "border-box", r.style
               .fontSize = "12px", r.style.fontFamily =
               "Arial, Verdana, sans-serif";
            var l = c.createElement("a");
            l.setAttribute("data-decontext-role",
                  "popup-open-control"), l.setAttribute("href",
                  "#"), l.setAttribute("target", "_blank"), l
               .innerHTML = "Open in a new window", s(l), r
               .appendChild(l);
            var i = c.createElement("a");
            i.setAttribute("data-decontext-role",
               "popup-dismiss-control"), i.setAttribute("href",
               "javascript:void(0);"), i.innerHTML = "Close", s(i), r.appendChild(
               i), n.appendChild(r);
            var a = c.createElement("iframe");
            a.setAttribute("data-decontext-role", "popup-body"), a
               .style.boxSizing = "border-box", a.style.width =
               "100%", a.style.border = "none", n.appendChild(a),
              p.addEventListener("click", function(t) {
                  (t.target === i || f() && t.target === d
                     .popupContainerEl) && (u(), t
                     .preventDefault())
               }),
              p.addEventListener("keydown", function(t) {
                  t.defaultPrevented || "Escape" === t.key &&
                  f() && (u(), t.preventDefault())
               }),
              c.querySelector("body")
               .appendChild(e), d.popupContainerEl = e, u(), p
               .addEventListener("resize", function() {
                  d.resizeTimeout || (d.resizeTimeout =
                     setTimeout(function() {
                        d.resizeTimeout = null, f() && h()
                     }, 66))
               }, !1)
         }();
      for (var t, e, o = c.querySelectorAll(
            '[data-decontext="true"]'), n = [], r = 0; r < o
         .length; ++r) n.push(a(o[r]));
      t = p.drexplain = p.drexplain || {}, ((e = t.addon = t
               .addon || {})
            .decontext = e.decontext || {})
         .open = function(t) {
            g(t)
         }, setTimeout(function() {
            ! function t(e) {
               for (var o = 0; o < e.length; ++o) {
                  var n = e[o];
                  y(n.el, n.calloutEl, n.options)
               }
               setTimeout(function() {
                  t(e)
               }, d.updateInterval)
            }(n)
         }, d.updateInterval)
   }, "loading" !== c.readyState ? t() : c.addEventListener(
      "DOMContentLoaded", t)
}(document, window);
//# sourceMappingURL=decontext.min.js.map
