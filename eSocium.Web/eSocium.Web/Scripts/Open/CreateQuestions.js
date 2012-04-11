$(document).ready(function () {
    var Question_Form_Storage;
    var Question_Mark_Storage;
    $(hasHeaderCheckbox).change(function () {
        $(Form).toggle(!this.checked);
        $('label[for="Form"]').toggle(!this.checked);
        $(Mark).toggle(!this.checked);
        $('label[for="Mark"]').toggle(!this.checked);
        if (this.checked) {
            Question_Form_Storage = $(Form).val();
            Question_Mark_Storage = $(Mark).val();
            $(Form).val("NOT NEEDED");
            $(Mark).val("NOT NEEDED");
        } else {
            $(Form).val(Question_Form_Storage);
            $(Mark).val(Question_Mark_Storage);
        }
    });
});