var PORTALAPP = angular.module("PORTAL", []);

window.odometerOptions = {
    auto: false, // Don't automatically initialize everything with class 'odometer'
    selector: '', // Change the selector used to automatically find things to be animated
    format: '', // Change how digit groups are formatted, and how many digits are shown after the decimal point
    duration: 1500, // Change how long the javascript expects the CSS animation to take
    theme: 'default', // Specify the theme (if you have more than one theme css file on the page)
    animation: 'count' // Count is a simpler animation method which just increments the value,
    // use it when you're looking for something more subtle.
};



JSBASE = {

    ROOT_URL: $("#hdnRoot").val(),
    AJAX_POST: function (action, controller, area, json, async, callbackSucesso) {

        var asyncTemp = true;
        if (async !== null && async !== undefined) {
            asyncTemp = async;
        }

        var path = "";
        if (area === undefined || area === "") {
            path = JSBASE.ROOT_URL + controller + "/" + action;
        }
        else {
            path = JSBASE.ROOT_URL + area + "/" + controller + "/" + action;
        }

        $.ajax({
            type: "POST",
            async: asyncTemp,
            data: json,
            url: path,
            // withCredentials: true,
            success: function (resposta) {

                console.log("RESULT: " + JSON.stringify(resposta));
                //if (resposta.Authenticated === false) {
                //    console.log("ERRO 2:" + JSON.stringify(resposta));
                //    swal({
                //        title: "Atenção",
                //        text: "Autenticação expirou :( (ERRO 2)",
                //        type: "error",
                //        confirmButtonClass: "btn-danger"
                //    });
                //    return;
                //}
                //else 
                if (resposta.errorMessage !== "") {

                    toastr.error("Não foi possível realizar sua requisição :( Tente mais tarde!! ", "Atenção", {
                        positionClass: "toast-top-center",
                        timeOut: 5e3,
                        closeButton: !0,
                        debug: !1,
                        newestOnTop: !0,
                        progressBar: !0,
                        preventDuplicates: !0,
                        onclick: null,
                        showDuration: "300",
                        hideDuration: "1000",
                        extendedTimeOut: "1000",
                        showEasing: "swing",
                        hideEasing: "linear",
                        showMethod: "fadeIn",
                        hideMethod: "fadeOut",
                        tapToDismiss: !1
                    });
                    console.log("ERROR JSBASE");
                    return;
                }
                else {
                    console.log("SUCCESS JSBASE");
                    if (jQuery.isFunction(callbackSucesso)) {
                        callbackSucesso(resposta);
                    }
                }
            },
            error: function (resposta) {
                console.log("ERRO:" + JSON.stringify(resposta));
                //swal({
                //    title: "Atenção",
                //    text: "Não foi possível realizar sua requisição :( Tente mais tarde!! (ERRO)",
                //    type: "error",
                //    confirmButtonClass: "btn-danger"
                //});
            }
        });
    },
    SHOW_MESSAGE: function (title, message, type) {

        if (type === "success") {
            toastr.success(message, title, {
                positionClass: "toast-top-center",
                timeOut: 5e3,
                closeButton: !0,
                debug: !1,
                newestOnTop: !0,
                progressBar: !0,
                preventDuplicates: !0,
                onclick: null,
                showDuration: "300",
                hideDuration: "1000",
                extendedTimeOut: "1000",
                showEasing: "swing",
                hideEasing: "linear",
                showMethod: "fadeIn",
                hideMethod: "fadeOut",
                tapToDismiss: !1
            });
        }
        if (type === "error") {
            toastr.error(message, title, {
                positionClass: "toast-top-center",
                timeOut: 5e3,
                closeButton: !0,
                debug: !1,
                newestOnTop: !0,
                progressBar: !0,
                preventDuplicates: !0,
                onclick: null,
                showDuration: "300",
                hideDuration: "1000",
                extendedTimeOut: "1000",
                showEasing: "swing",
                hideEasing: "linear",
                showMethod: "fadeIn",
                hideMethod: "fadeOut",
                tapToDismiss: !1
            });
        }
    }
};