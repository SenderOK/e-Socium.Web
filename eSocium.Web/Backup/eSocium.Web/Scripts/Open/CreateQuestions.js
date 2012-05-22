$(document).ready(
function () {
    var Question_Form_Storage = $(Form).val();
    var Question_Mark_Storage = $(Mark).val();

    function SetCheckboxState(is_checked) {
        is_checked = $(hasHeaderCheckbox).is(':checked');
        $(Form).toggle(!is_checked);
        $('label[for="Form"]').toggle(!is_checked);
        $(Mark).toggle(!is_checked);
        $('label[for="Mark"]').toggle(!is_checked);
        if (is_checked) {
            Question_Form_Storage = $(Form).val();
            Question_Mark_Storage = $(Mark).val();
            if ((Question_Form_Storage).trim().length == 0) {
                $(Form).val("NOT NEEDED");
            }
            if ((Question_Mark_Storage).trim().length == 0) {
                $(Mark).val("NOT NEEDED");
            }
        } else {
            $(Form).val(Question_Form_Storage);
            $(Mark).val(Question_Mark_Storage);
        }
    }

    SetCheckboxState();

    $(hasHeaderCheckbox).change(function () {
        SetCheckboxState();
    });
}
);