
function scrollToBottom(name) {
    document.getElementsByClassName(name)[0].scrollTop = document.getElementsByClassName(name)[0].scrollHeight;
}
function blazorGetTimezoneOffset() {
    return new Date().getTimezoneOffset();
}

function dynmapAppendCssRemoveControls() {
    var cssLink = document.createElement("link");
    cssLink.href = "dynmap-removecontrols.css";
    cssLink.rel = "stylesheet";
    cssLink.type = "text/css";
    frames['iframedynmap1'].document.head.appendChild(cssLink);
}