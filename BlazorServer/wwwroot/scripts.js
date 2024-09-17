
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
    frames['iframe1'].document.head.appendChild(cssLink);
}

function invokeOpenPlayerList() {
    frames['iframe1'].document.contentWindow.openPlayerList();
}