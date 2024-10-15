document.querySelector('.table__checkbox').addEventListener('change', function () {
    const checkboxes = document.querySelectorAll('.checkbox__selected-user');
    checkboxes.forEach(function (checkbox) {
        checkbox.checked = this.checked;
    }.bind(this));
});