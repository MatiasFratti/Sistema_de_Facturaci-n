$(document).ready(function () {
    
    var rutt = $("#rut_input");
    var nombre_input = $("#nombre_input");
    var direccion_cli_input = $("#direccion_cli_input");

    $(nombre_input).autocomplete({
        source: "../Clientes/GetSearchValue2",
        dataType: "json"
       
    });
    $(function () {
        $("#fecha").datepicker({ dateFormat: 'dd-m-yy' });

    });
    var d = new Date();
    var dia = d.getDate();
    var mes = d.getMonth()+1;
    var ano = d.getFullYear();

    console.log(dia + mes + ano);
    $('#fecha_input').val(dia+"/"+mes+"/"+ano);
    //$('#mes_input').val(mes + 1);
    //$('#año_input').val(ano);

    var rutt = $("#rut_input");
    var nombre_input = $("#nombre_input");
    var direccion_cli_input = $("#direccion_cli_input");

    function completado() {
        if (nombre_input.val() != "") {
            $.ajax({
                url: "AutoCliente",
                type: 'GET',

                data: { term: $(nombre_input).val() },
                success: function (resultado) {
                    ($.map(resultado, function (item) {
                        return { rutt: $(rutt.val(item.rut)), direccion: direccion_cli_input.val(item.direccion) }
                        console.log(resultado + "Hola");
                    }))

                },
                error: function (xhr, status, error) {
                    alert("Error");
                }
            });
        }
    }


    $(nombre_input).focusout(function () {
        completado();
    });


    
    for (let i = 1; i < 10; i++) {
        if ($(`#detalle_input${i}`).val() != "0" && $(`#detalle_input${i}`).val() == "") {

            $(`#detalle_input${i}`).autocomplete({
                source: "../Productos/GetSearchValue",
                dataType: "json"

            });

            $(`#detalle_input${i}`).focusout(function () {
                function completado2() {
                    $.ajax({
                        url: "../Productos/MultiplicaCantidadPrecio",
                        type: "GET",
                        data: { term: $(`#detalle_input${i}`).val() },
                        success: function (resultado) {
                            ($.map(resultado, function (item) {
                                var cant = $(`#cantidad_input${i}`).val();
                                var total = cant * (item.Precio);
                                (item.Precio + "  " + cant + "  " + total);
                                if ($(`#detalle_input${i}`).val() != ""/* || $(`#cantidad_input${i}`).val() == "" || $(`#cantidad_input${i}`).val() != ""*/) {
                                    $(`#precio_input${i}`).val(item.Precio.toFixed(2))
                                }

                                return { precio: $(`#total_input${i}`).val(total.toFixed(2)) }

                            }))
                        },
                        error: function (xhr, status, error) {
                                alert("Error");
                        }
                    })
                }
                completado2();
               

                $(`#cantidad_input${i}`).focusout(function () {
                    var cant = $(`#cantidad_input${i}`).val();
                    var precio = $(`#precio_input${i}`).val()
                    var total = parseFloat(cant) * parseFloat(precio);
                    $(`#total_input${i}`).val(total.toFixed(2));
                    console.log(cant + " canti " + precio + " total ");
                });
                

            });

        }


    }
    var sum;
    var sumTotal = 0;
    //var unitarioDolar = true;
    var unitarioPesos = true;
    var IVA = 0.22;
    var TOTAL ;
    var Dolar_en_pesos = 32.6;
    var controlDolar = 1;
    var controlPesos = 1;
    var unitarioDolar;

    //$("#moneda").click(function () {

    //    if ($("#moneda").val() == "U$S") {
    //        if (unitarioDolar == false) {
    //            unitarioDolar = true;
    //        }
    //    }
    //    if ($("#moneda").val() == 1) {
    //        if (unitarioPesos == false) {
    //            unitarioPesos = true;
    //        }
    //    }
    //});

    $("#calcular").click(function () {
        var subtotal;
        function sumar() {
            for (var i = 1; i < 9; i++) {
                if ($(`#total_input${i}`).val() != "") {
                    sum = parseFloat($(`#total_input${i}`).val());
                    sumTotal = parseFloat(sumTotal) + parseFloat(sum);
                   

                    console.log(sumTotal + "  Hola");
                }
                if ($(`#total_input${i}`).val() == 0) {
                    $(`#total_input${i}`).val("");
                }
            }
            //var IVA = 0.22;
            //var TOTAL = (parseFloat(IVA) * parseFloat(sumTotal)) + parseFloat(sumTotal);
            //var Dolar_en_pesos = 32.6;
            TOTAL = (parseFloat(IVA) * parseFloat(sumTotal)) + parseFloat(sumTotal);
            console.log(sumTotal + "sumatotal");

            if ($("#moneda").val() == "U$S" && unitarioPesos == true && $("#total_precio_input").val() != controlDolar) {
                $("#subtotal_input").val(sumTotal.toFixed(2));
                $("#iva_input").val(parseFloat((sumTotal) * (IVA)).toFixed(2));
                $("#total_precio_input").val(TOTAL.toFixed(2));
                controlDolar = $("#total_precio_input").val(TOTAL.toFixed(2));;
                
                console.log("unitariopesosa");
            }

            if ($("#moneda").val() == "U$S" && controlPesos != 1 && unitarioPesos == false) {
               
                for (var i = 1; i < 9; i++) {
                    if ($(`#precio_input${i}`).val() != "") {
                        console.log(controlPesos + "er " + unitarioPesos);
                        unitarioDolar = parseFloat(($(`#precio_input${i}`).val()) / (Dolar_en_pesos)).toFixed(2);
                        $(`#precio_input${i}`).val(unitarioDolar);
                        var totalP = $(`#total_input${i}`).val() / (Dolar_en_pesos);
                        $(`#total_input${i}`).val(totalP.toFixed(2));
                       
                        subtotal = parseFloat(sumTotal / (Dolar_en_pesos)).toFixed(2);
                        $("#subtotal_input").val((subtotal));
                        var iva = parseFloat(((sumTotal) * (IVA)) / (Dolar_en_pesos)).toFixed(2);
                        $("#iva_input").val(iva);
                        var t = (TOTAL / Dolar_en_pesos).toFixed(2);
                        $("#total_precio_input").val(parseFloat(t));
                    }
                     
                }
               
                unitarioPesos = true;
                controlPesos = 1; 
                    
            }
               
                //sumTotal = 0;
            
            //TOTAL = (parseFloat(IVA) * parseFloat(sumTotal)) + parseFloat(sumTotal);
            if ($("#moneda").val() == 1 && unitarioPesos == true && controlPesos == 1) {
                       // 1 = pesos Uruguayos
                $("#subtotal_input").val(((sumTotal) * (Dolar_en_pesos)).toFixed(2));
                $("#iva_input").val(parseFloat((sumTotal) * (IVA) * (Dolar_en_pesos)).toFixed(2));
                $("#total_precio_input").val(((TOTAL) * (Dolar_en_pesos)).toFixed(2));
                controlPesos = 0;
                unitarioPesos = false;
                
               
               
                function preciopesos() {

                    for (var i = 1; i < 9; i++) {
                        if ($(`#precio_input${i}`).val()!="" ){
                            unitarioDolar = parseFloat(($(`#precio_input${i}`).val()) * (Dolar_en_pesos)).toFixed(2);
                            $(`#precio_input${i}`).val(unitarioDolar);
                            var dolarTotal = parseFloat($(`#total_input${i}`).val()) * (Dolar_en_pesos);
                            $(`#total_input${i}`).val(dolarTotal.toFixed(2));
                        }
                        console.log(controlPesos + "33 " + unitarioPesos);
                       
                    }
                   
                }
               
                preciopesos();
                
                          
                //sumTotal = 0;
               
                
            }
            sumTotal = 0;
        }
        
        
        sumar();
       

    });
    $("#facturacion").click(function (e) {
    
        var moneda = $("#moneda").val();
        var subtotal = $("#subtotal_input").val();
        var total = $("#total_precio_input").val();
       
        
        var fact = confirm("¿Desea emitir la factura?");
        if (!fact) {
            e.preventDefault();
            alert("No se ha emitido la factura");
                
        }
        var _cantidad=[];
        var _precio=[];
        var _total=[];
        for (var i = 0; i < 8; i++) {
            if ($(`#cantidad_input${i}`).val() != "") {
                _cantidad.push($(`#cantidad_input${i}`).val());
            }
            if ($(`#precio_input${i}`).val() != "") {
                _precio.push($(`#precio_input${i}`).val());
            }
            if ($(`#total_input${i}`).val() != "")
            _total.push($(`#total_input${i}`).val());
        }
        if (_cantidad.length != _precio.length || _precio.length != _total.length) {
            e.preventDefault();
            alert("Verifique los campos de cantidad, precio y total, pues algo no coincide");
        }
        if (_cantidad.length < 1) {
            e.preventDefault();
            alert("Campo 'Cantidad' no puede estar vacío")
        }
        if (_precio.length < 1) {
            e.preventDefault();
            alert("Campo 'Precio' no puede estar vacío")
        }
        if (_total.length < 1) {
            e.preventDefault();
            alert("Campo 'Total' no puede estar vacío")
        }
        if ($("#nroFactura").val() == "" || $("#rut_input").val() == "" || $("#nombre_input").val() == "" || $("#fecha_input").val() == "" /*|| $("#mes_input").val() == "" || $("#año_input").val() == "" */|| $("#subtotal_input").val() == "" || $("#iva_input").val() == "" || $("#total_precio_input").val() == "") {
            e.preventDefault();
            alert("Campos importantes vacíos, chequea Nro de Factura, Fecha, nombre de cliente, rut, subtotal,iva y total");
        }
        //if (parseInt($("#dia_input").val()) > dia || parseInt($("#dia_input").val()) <= 0 || parseInt($("#dia_input").val()) > 31 || parseInt($("#mes_input").val()) > (mes+1) || parseInt($("#mes_input").val()) <= 0 || parseInt($("#mes_input").val()) > 12 || parseInt($("#año_input").val()) > ano || parseInt($("#año_input").val()) < ano) {
        //    e.preventDefault();
        //    alert("La fecha no esta bien")
        //    console.log(parseInt($("#dia_input").val()) + "  " + dia +"" + parseInt($("#mes_input").val()) + " " + (mes+1)+" " + parseInt($("#año_input").val()) +" "+ ano);
        //}
        console.log("c" + _cantidad + " p " + _precio + " t " + _total);
        
    });
});