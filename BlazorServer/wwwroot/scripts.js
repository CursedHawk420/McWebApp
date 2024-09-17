
function scrollToBottom(name) {
    document.getElementsByClassName(name)[0].scrollTop = document.getElementsByClassName(name)[0].scrollHeight;
}
function blazorGetTimezoneOffset() {
    return new Date().getTimezoneOffset();
}

function dynmapRemoveControls() {
    var iframe = document.getElementById("iframe1");
    var elmnt = iframe.contentWindow.document.getElementsByClassName("control-bar")[0];
    var elmnt = iframe.contentWindow.document.getElementsByClassName("control-bar")[1];
    var elmnt = iframe.contentWindow.document.getElementsByClassName("control-bar")[2];
    elmnt.style.display = "none";
    elmnt.style.visibility = "hidden";
}

function dynmapAppendCss() {
    var cssLink = document.createElement("link");
    cssLink.href = "dynmap-custom.css";
    cssLink.rel = "stylesheet";
    cssLink.type = "text/css";
    frames['iframe1'].document.head.appendChild(cssLink);
}

