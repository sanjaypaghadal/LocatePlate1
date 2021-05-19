$(function () {


    function setCookie(name, value, days) {
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    }

    function getCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    var x = getCookie('timezone');
    if (x == undefined || x == '') {
        var dateString = new Date();
        var offset = dateString.getTimezoneOffset();
        setCookie('timezone', offset, 7);
    }

    var locationId = getCookie('LocationId');
    //var currenturi = decodeURIComponent(window.location.href);

    //if (currenturi.indexOf("/restaurants") != -1) {
    //    var locationName = currenturi.split("/restaurants")[0].split("/")[3];
    //    $(`#header-locationId option[value="${locationName}"`).attr('selected', 'selected');
    //}
    //else
    if (locationId != undefined && locationId != '') {
        $(".header-locationId").val(locationId);
    }

    $(".header-locationId").on('change', function () {
        $.ajax({
            url: `/SetLocation/${$(this).val()}/${$("#header-locationId option:selected").text()}`,
            type: 'Get',
            success: function () {
                window.location = "/home";
            }
        });
    });

    //var currentLocation = getCookie("CurrentLocation");
    //if (currentLocation == null)

});