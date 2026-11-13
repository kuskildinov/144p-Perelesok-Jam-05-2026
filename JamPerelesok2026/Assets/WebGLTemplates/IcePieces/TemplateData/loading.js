(function () {
  "use strict";
  var loading = document.getElementById("ip-loading");
  var ring = document.getElementById("ip-ring");
  var circle = document.getElementById("ip-ring-progress");
  var percent = document.getElementById("ip-percent");
  var status = document.getElementById("ip-status");
  var error = document.getElementById("ip-error");
  var errorMessage = document.getElementById("ip-error-message");
  var retry = document.getElementById("ip-retry");
  var fullscreen = document.getElementById("ip-fullscreen");
  var canvas = document.getElementById("unity-canvas");
  var circumference = 2 * Math.PI * 51;
  var instance = null;
  var failed = false;

  circle.style.strokeDasharray = String(circumference);
  circle.style.strokeDashoffset = String(circumference);

  function updateProgress(value) {
    var progress = Math.max(0, Math.min(1, Number(value) || 0));
    var valuePercent = Math.round(progress * 100);
    circle.style.strokeDashoffset = String(circumference * (1 - progress));
    percent.textContent = valuePercent + "%";
    ring.setAttribute("aria-valuenow", String(valuePercent));
    status.textContent = progress >= 1 ? "Starting game..." : "Loading game...";
  }

  function fail(message) {
    if (failed) return;
    failed = true;
    loading.hidden = true;
    errorMessage.textContent = String(message || "Unknown error");
    error.hidden = false;
    fullscreen.hidden = true;
    console.error("Ice Pieces WebGL:", message);
  }

  function start(config) {
    if (typeof createUnityInstance !== "function") {
      fail("Unity loader did not initialize.");
      return;
    }
    status.textContent = "Loading game...";
    try {
      createUnityInstance(canvas, config, updateProgress).then(function (unityInstance) {
        if (failed) return;
        instance = unityInstance;
        updateProgress(1);
        loading.classList.add("is-hidden");
        loading.addEventListener("transitionend", function () { loading.hidden = true; }, { once: true });
        fullscreen.hidden = false;
      }).catch(function (reason) {
        fail(reason && reason.message ? reason.message : String(reason));
      });
    } catch (reason) {
      fail(reason && reason.message ? reason.message : String(reason));
    }
  }

  retry.addEventListener("click", function () { window.location.reload(); });
  fullscreen.addEventListener("click", function () {
    if (!instance) return;
    if (document.fullscreenElement && document.exitFullscreen) {
      document.exitFullscreen().catch(function (e) { console.warn(e); });
    } else if (typeof instance.SetFullscreen === "function") {
      instance.SetFullscreen(1);
    }
  });

  window.addEventListener("error", function (event) {
    if (!instance && !failed && event.target && event.target.tagName === "SCRIPT") {
      fail("A required script could not be loaded.");
    }
  }, true);

  window.IcePiecesLoader = { start: start, fail: fail };
}());
