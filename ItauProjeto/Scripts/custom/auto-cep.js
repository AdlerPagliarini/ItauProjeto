jQuery(function ($) {
    $("#CEP").change(function () {
        var cep_code = $(this).val();
        if (cep_code.length <= 0) return;
        $.get("http://apps.widenet.com.br/busca-cep/api/cep.json", { code: cep_code },
           function (result) {
               if (result.status != 1) {
                   alert("Não foi possível localizar o CEP, por favor insera manualmente as informações de endereço.");
                   return;
               }
               if (result.code != "" && result.code != null)
                   $("input#CEP").val(result.code);

               if (result.state != "" && result.state != null)
                   $("input#Estado").val(result.state);

               if (result.city != "" && result.city != null)
                   $("input#Cidade").val(result.city);

               if (result.district != "" && result.district != null)
                   $("input#Bairro").val(result.district);

               if (result.address != "" && result.address != null)
                    $("input#Endereco").val(result.address);
           });
    });
});