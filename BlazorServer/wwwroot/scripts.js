
function scrollToBottom(name) {
    document.getElementsByClassName(name)[0].scrollTop = document.getElementsByClassName(name)[0].scrollHeight;
}
function blazorGetTimezoneOffset() {
    return new Date().getTimezoneOffset();
}