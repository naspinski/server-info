
function Modal(modalWindow, triggerCollection) {
    $(modalWindow).jqm({
        trigger: $(triggerCollection),
        onShow: function (h) { h.w.slideDown(); },
        onHide: function (h) { h.w.slideUp('medium', function () { if (h.o) h.o.remove(); }); }
    });
}