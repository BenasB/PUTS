function setHeight(jq_in) {
    jq_in.each(function (index, elem) {
        elem.style.height = elem.scrollHeight + 'px';
    });
}
setHeight($('textarea'));