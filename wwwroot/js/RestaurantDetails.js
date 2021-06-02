try {
    var menuItem = "";
    $('body').on('click', '.add-to-order', function () {
        menuItem = $(this).attr("data-menuitem");
        var url = $("#url").val();
        var cityName = $("#CityName").val();
        $("#add-to-order-model").html("");
        $.ajax({
            type: "GET",
            url: `/getaddtoorder/${menuItem}/${url}/${cityName}`,
            success: function (orders) {
                $("#add-to-order-placeholder").html(orders);
                $("#add-to-order-model").modal('show');
                hideLoader();
            },
            error: function (response) {
                hideLoader();
            }
        });
    });


    $('body').on('click', '.prepare-order', function () {
        var formdata = $("#addtoorder").serialize();
        var dateval = $("#slotdate").val();
        showLoader();
        $.ajax({
            type: "POST",
            url: `/addtoorder/${dateval}`,
            data: formdata,
            success: function (orders) {
                $("#order-contianer").html(orders);
                $(`td[data-menuitem="${menuItem}"]`).removeClass("text-primary float-right add-style btn cursor-pointer add-to-order bold-600 bg-outline-primary");
                $(`td[data-menuitem="${menuItem}"]`).addClass("text-white bg-primary float-right btn cursor-pointer added-to-order bold-600");
                $(`td[data-menuitem="${menuItem}"]`).html("Added");
                //$(`td[data-menuitem="${menuItem}"]`).replaceWith(addedmarkup);
                hideLoader();
                $("#add-to-order-model").modal('hide');
            },
            error: function (response) {
                console.log(response);
                hideLoader();
                $("#add-to-order-model").modal('hide');
            }
        });
    });

    $('body').on('click', '.increate-button', function () {
        showLoader();
        var el = $(this).siblings('.number');
        var num = el.val();
        var menuItem = $(this).siblings('.number').attr('data-menuitem');
        var value = parseInt(num, 10);
        value = isNaN(value) ? 0 : value;
        value++;

        var restaurantId = $("#currentrestaurant").val();
        var dateval = $("#slotdate").val();
        $.ajax({
            type: "GET",
            url: `/addquantity/${menuItem}/${restaurantId}/${value}/${dateval}`,
            xhrFields: {
                withCredentials: true
            },
            success: function (orders) {
                $("#order-contianer").html(orders);
                hideLoader();
            },
            error: function (response) {
                console.log(response);
                hideLoader();
            }
        });
        el.val(value);
    });

    $('body').on('click', '.decrease-button', function () {
        showLoader();
        var el = $(this).siblings('.number');
        var num = el.val();
        var menuItem = $(this).siblings('.number').attr('data-menuitem');
        var value = parseInt(num, 10);
        value = isNaN(value) ? 0 : value;
        if (value === 1) {
            $(`td[data-menuitem="${menuItem}"]`).removeClass("text-white bg-primary float-right btn cursor-pointer added-to-order bold-600");
            $(`td[data-menuitem="${menuItem}"]`).addClass("text-primary float-right add-style btn cursor-pointer add-to-order bold-600 bg-outline-primary");
            $(`td[data-menuitem="${menuItem}"]`).html("Add +");
            //$(`td[data-menuitem="${menuItem}"]`).replaceWith(`<td class="" data-menuitem="${menuItem}"</td>`);
            /// $(this).parent().parent().parent().remove();
        }
        value--;

        //var url = $("#url").val(); 
        //var cityName = $("#CityName").val();
        var restaurantId = $("#currentrestaurant").val();
        var dateval = $("#slotdate").val();
        $.ajax({
            type: "GET",
            url: `/removeorder/${menuItem}/${restaurantId}/${value}/${dateval}`,
            xhrFields: {
                withCredentials: true
            },
            success: function (orders) {
                $("#order-contianer").html(orders);
                hideLoader();
            },
            error: function (response) {
                hideLoader();
                console.log(response);
            }
        });

        el.val(value);
    });
    $('body').on('click', '#apply-promo', function () {
        showLoader();
        var promocode = $("#promo-code").val();
        var restaurantId = $("#RestaurantModelId").val();
        var dateval = $("#slotdate").val();
        $.ajax({
            type: "GET",
            url: `/applypromo/${promocode}/${restaurantId}/${dateval}`,
            success: function (orders) {
                $("#order-contianer").html(orders);
                hideLoader();
            },
            error: function (response) {
                console.log(response);
                hideLoader();
            }
        });
    });
    $('body').on('click', '#contactusSaveData', function () {
        //var url = '@Url.Action("SaveUserQuestion", "Home")';
        var url = "Home/SaveUserQuestion";
        var result = {
            "Name": $("#FullNameContact").val(),
            "Email": $("#InputEmailContact").val(),
            "Telephone": $("#InputMobileContact").val(),
            "Message": $("#TextareaContact").val(),
        };
        $.ajax({
            url: url,
            type: 'post',
            data: result,
            success: function (response) {
                if (!response) {
                    alert("Thank you for contacting us.");
                }
            }
        });
    });
    $('body').on('click', '#remove-promo', function () {
        showLoader();
        $("#promo-code").val('');
        var restaurantId = $("#RestaurantModelId").val();
        var dateval = $("#slotdate").val();
        $.ajax({
            type: "GET",
            url: `/removepromo/${restaurantId}/${dateval}`,
            success: function (orders) {
                $("#order-contianer").html(orders);
                hideLoader();
            },
            error: function (response) {
                console.log(response);
                hideLoader();
            }
        });
    });

    $('body').on('click', '#BookTable', function () {
        var isError = false, msg = "";

        var resType = $("#ReservationType").val();
        $("#partysize").removeClass("btn-outline-primary");
        $("#slots").removeClass("btn-outline-primary");

        if (resType == "0") {
            if ($("#partysize").val() == "0") {
                $("#partysize").addClass("btn-outline-primary");
                msg = "Please select Number of guests.";
                isError = true;
            }
            if ($("#slots").val() == "0" || $("#slots").val() == "Close") {
                $("#slots").addClass("btn-outline-primary");
                msg = (msg == "" ? "Please select time." : msg);
                isError = true;
            }
        }
        if (resType == "1") {
            if ($("#slots").val() == "0" || $("#slots").val() == "Close") {
                $("#slots").addClass("btn-outline-primary");
                msg = (msg == "" ? "Please select time." : msg);
                isError = true;
            }
        }

        if (!isError) {
            var orderData = {
                'RestaurantId': $("#RestaurantModelId").val(),
                'PartySize': $("#partysize").val(),
                'Date': $("#slotdate").val(),
                'StartTime': $("#slots").val(),
            }
            $.ajax({
                url: "/Home/OrderIsValid",
                type: 'POST',
                data: orderData,
                success: function (response) {
                    if (!response) {
                        $('#modalAlert').modal('show');
                        $('#alertmodelbody').text("This Table with same time already booked check after 2 hours or you can choose different time");
                    }
                    else {
                        var redirect = $(this).attr("data-redirect");
                        console.log(redirect);
                        if (redirect != undefined && redirect != "") {
                            var querystring = "";
                            //if (window.location.href.indexOf('?') > -1) {
                            //    querystring = "?" + window.location.href.split('?')[1];
                            //}
                            isError = true;
                            window.location.href = redirect + querystring;
                        } else {

                            $("#reservation-form").submit();
                        }
                    }
                }
            });

        }
        else {
            /*<a data-toggle="modal" data-target="#modalDelete"></a>*/
            $('#modalAlert').modal('show');
            $('#alertmodelbody').text(msg);
        }
    });

} catch (err) {
    console.log(err);
    hideLoader();
}



$(document).ready(function () {
    //$("#loginplaceholder").load('/account/login/', function () { });
    //$("#registerplaceholder").load(`/account/register`, function () { });
    $('.star-button').change(function () {
        showLoader();
        if (this.checked) {
            var rate = $(this).val();
            var restaurantId = $("#RestaurantModelId").val();
            var ratingType = $(this).attr("rating-type");

            $.ajax({
                url: `/rating/${rate}/${restaurantId}/${ratingType}`,
                type: 'Get',
                success: function () {
                    ///window.location = "/home";
                    hideLoader();
                },
                error: function (err) {
                    hideLoader();
                }
            });
        }
    });

    $("#submit-review").click(function () {
        showLoader();
        $.ajax({
            url: `/review/restaurant`,
            type: 'POST',
            data: $("#revew-form").serialize(),
            success: function (data) {
                ///window.location = "/home";
                $("#review-area").val("");
                $("#all_user_placeholder").html(data);
                hideLoader();
            },
            error: function (err) {
                hideLoader();
            }
        });
    });

    $(".order-type").click(function () {
        console.log("order");
        var id = $(this).attr("id");
        var value = 0;
        switch (id) {
            case "Dine-In":
                $("#party-section").css("display", "block");
                value = 0;
                break;
            case "Take-Away":
                $("#party-section").css("display", "none");
                value = 1;
                break;
        }

        $("#ReservationType").val(value);
    });

    var slotdate = $("#slotdate").val();
    var partySize = $("#partysize").val();

    $("#slotdate").on('change', function () {
        showLoader();
        slotdate = $("#slotdate").val();
        fillSlots();
    });
    $("#partysize").on('change', function () {
        showLoader();
        partySize = $("#partysize").val();
        fillSlots();
    });

    var dateStr = getQueryVariable("Date");
    var GuestQuery = getQueryVariable("Guest");
    var TimeSlot = getQueryVariable("Time");

    if (dateStr != "") {
        if (GuestQuery != "") {
            partySize = GuestQuery;
            $("#partysize").val(partySize)
        }
        slotdate = dateStr;
        $("#slotdate").val(slotdate);
        fillSlots();
    }

    $("#slots").on('change', function () {
        refreshUrl();
    });


    function fillSlots() {
        var regionsSelect = $("#slots");
        regionsSelect.empty();
        var restaurantId = $("#currentrestaurant").val();
        var current = new Date();
        console.log(current);
        //    slotdate = new Date($`{slotdate}`).setHours(current.getHours(), current.getMinutes(), current.getSeconds(), current.getMilliseconds());
        $.ajax({
            type: "GET",
            url: `/availability/${restaurantId}/${partySize}/${slotdate} ${current.getHours()}:${current.getMinutes()}:${current.getSeconds()}`,
            success: function (regions) {
                if (regions != null && !jQuery.isEmptyObject(regions)) {
                    $.each(regions, function (index, region) {
                        if (region.value == TimeSlot)
                            $("#slots").append("<option Selected value='" + region.value + "'>" + region.text + "</option>");
                        else
                            $("#slots").append("<option value='" + region.value + "'>" + region.text + "</option>");
                    });
                    if (regions.length == 1)
                        $("#slots").append("<option value='Close'>Close</option>");
                } else {
                    regionsSelect.append($('<option/>', {
                        value: '0',
                        text: "No Slots"
                    }));
                }
                refreshUrl();
                hideLoader();
                // if (response == true) window.location.reload(true);
            },
            error: function (response) {
                $("#login-data-form-Summary").val(response);
                hideLoader();
            }
        });
    }

    function refreshUrl() {
        var slot = $("#slots").val();
        if (slot != null && slot != undefined && slot != 0 && slot != "") {
            TimeSlot = slot;
        }
        let newquerystring = `?Guest=${partySize}&Date=${slotdate}&Time=${TimeSlot}`;

        var currenturi = window.location.href.split('?')[0];
        let stateObj = { id: "100" };
        window.history.pushState(stateObj,
            "Page", currenturi + newquerystring);
    }
});