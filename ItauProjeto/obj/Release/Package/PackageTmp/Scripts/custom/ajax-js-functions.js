/********************************************JavaScript********************************************
**********************************************JavaScript********************************************
***********************************************JavaScript********************************************
************************************************JavaScript********************************************
*************************************************JavaScript********************************************
**************************************************JavaScript********************************************/
$(document).ready(function () { });

/*janela de alertConfirm costumizada*/
function OnConfirm(text, func) {
    $('#btnConfirmOk').unbind('click');
    $('#btnConfirmOk').bind('click', function () {
        func(true);
        $('#excluindo').fadeOut();
    });
    $('#excluindo .text').text(text);

    $('#btnConfirmCancel').unbind('click');
    $('#btnConfirmCancel').bind('click', function () {
        func(false);
        $('#excluindo').fadeOut();
    });
    $('#excluindo').fadeIn();
}
/*janela de alertConfirm costumizada*/


/*janela de alert costumizada*/
function alert(text) {
    $('#excluindoAlert').fadeIn();
    $('#excluindoAlert .text').text(text);
    $('#btnConfirmAlert').unbind('click');
    $('#btnConfirmAlert').bind('click', function () {
        $('#excluindoAlert').fadeOut();
    });
}

function alertReload(text, timeReload) {
    $('#excluindoAlert').fadeIn();
    $('#excluindoAlert .text').text(text);
    $('#btnConfirmAlert').unbind('click');
    $('#btnConfirmAlert').bind('click', function () {
        $('#excluindoAlert').fadeOut();
        setTimeout(function () { location.reload(); }, timeReload);
    });
}
/*janela de alert costumizada*/


/*Utilizado para exclusão do cliente*/
function excluindoCliente() {

    //$("#btnConfirmOkCliente").click(function () {});

    $('#btnConfirmCancelCliente').unbind('click');
    $('#btnConfirmCancelCliente').bind('click', function () {
        $('#excluindoCliente').fadeOut();
    });
    $('#excluindoCliente').fadeIn();

}//OnConfirm(text, func)

function SuccessRequest() {
    $('#excluindoCliente').fadeOut();
    alertReload("Perfil e arquivos excluídos com sucesso.", 2000);
}

function FailRequest() {
    alertReload("Não foi possível excluir seu perfil.");
}
/*Utilizado para exclusão do cliente*/




/********************************************AJAX********************************************
**********************************************AJAX********************************************
***********************************************AJAX********************************************
************************************************AJAX********************************************
*************************************************AJAX********************************************
**************************************************AJAX********************************************/

/*$(document).ready(function () { });*/

/*
function excluirConta(id, urlMetodo) {

    formData = "{ \"id\" : \"" + id + "\"}";

    var confirmText = "Deseja realmente excluir a sua conta? Está ação não poderá ser revertida.";

    // chama um confirm costumizado com callback
    OnConfirm(confirmText, function (_isContinue) {
        if (_isContinue) {
            $.ajax({
                type: "POST",
                data: formData,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: urlMetodo,
                success: function (data) {
                    var array = $.parseJSON(data);
                    $.each(array, function (key, val) {
                        $.each(val, function (keyy, item) {
                            var resultado = item.sucesso;
                            if (resultado == "OK") {
                                alertReload("Sua conta e arquivos foram excluídos com sucesso. Você será redirecionado para página inicial em 5 segundos.", 5000);
                                //location.reload();
                            } else {
                                alertReload("Não foi possível excluir sua conta.");
                            }
                        });
                    });
                },
                error: function (xhr, err) {
                    alert("Falha na exclusão da sua conta, por favor entre em contato com o adminstrador do sistema.");
                }
            });
        }
    });
}//excluirConta*/