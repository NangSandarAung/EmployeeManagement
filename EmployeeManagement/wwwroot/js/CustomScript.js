function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = "deleteSpan_" + uniqueId;
    var confirmDelete = "confirmDelete_" + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDelete).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDelete).hide();
    }
}