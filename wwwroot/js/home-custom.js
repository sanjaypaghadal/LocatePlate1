
var swiper = new Swiper('.box-shadows .swiper-container', {
    slidesPerView: 1,
    spaceBetween: 10,
    loop: true,
    centeredSlides: true,
    autoplay: {
        delay: 2500,
        disableOnInteraction: false,
    },
    pagination: {
        el: '.swiper-pagination',
        clickable: true,
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});

var swiper2 = new Swiper('.swiper-2 .swiper-container', {
    slidesPerView: 5,
    spaceBetween: 12,
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});

var swiper3 = new Swiper('.card-restaurants .swiper-container', {
    slidesPerView: 5,
    spaceBetween: 12,
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
    pagination: {
        el: '.swiper-pagination',
        clickable: true,
    },
    breakpoints: {
        480: {
            slidesPerView: 2,
            spaceBetween: 12,
        },
        640: {
            slidesPerView: 3,
            spaceBetween: 12,
        },
        786: {
            slidesPerView: 3,
            spaceBetween: 12,
        },
        991: {
            slidesPerView: 3,
            spaceBetween: 12,
        }
    },
});

$("#Location").click(function () {
});
$('.zoom-img').picEyes();
$(".signuptext").click(function () {
    $("#SignupBox").css("display", "block");
    $("#LoginBox").css("display", "none");
    $("#logintext").removeClass("option-bottom-style");
    $(".signuptext").addClass("option-bottom-style");
});
$("#logintext").click(function () {
    $("#LoginBox").css("display", "block");
    $("#SignupBox").css("display", "none");
    $(".signuptext").removeClass("option-bottom-style");
    $(this).addClass("option-bottom-style");
});
$(".datepicker").datepicker({
    dateFormat: "M d,yy",
    minDate: 0
}).datepicker("setDate", new Date());
function split(val) {
    return val.split(/,\s*/);
}
function extractLast(term) {
    return split(term).pop();
}

function showLoader() {
    console.log("log start");
    $('.loader').show();
    /* $('.loader').css("display","block!important");*/
}

function hideLoader() {
    console.log("log end");
    $('.loader').hide();
    /*  $('.loader').css("display", "none!important");*/
}

hideLoader();


// get the anchor link buttons
const menuBtn = $('.nav-pills .nav-link');
// when each button is clicked
menuBtn.click(() => {
    // set a short timeout before taking action
    // so as to allow hash to be set
    setTimeout(() => {
        // call removeHash function after set timeout
        removeHash();
    }, 5); // 5 millisecond timeout in this case
});

// removeHash function
// uses HTML5 history API to manipulate the location bar
function removeHash() {
    history.replaceState('', document.title, window.location.origin + window.location.pathname + window.location.search);
}
const menutabbed = $(".tabbed-section");
menutabbed.click(() => {
    // set a short timeout before taking action
    // so as to allow hash to be set
    setTimeout(() => {
        // call removeHash function after set timeout
        removeHash();
    }, 5); // 5 millisecond timeout in this case
});



function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        if (decodeURIComponent(pair[0]) == variable) {
            return decodeURIComponent(pair[1]);
        }
    }
    return "";
    console.log('Query variable %s not found', variable);
}

$("#btnFilterMobile").click(function () {
    $(".filter-div-head").css("display", "block");
});
$(".mobile-filter-done button").click(function () {
    $(".filter-div-head").css("display", "none");
});
$(".give-review-section label").click(function () {
    $(this).parent().find("label").css({ "color": "#eee" });
    $(this).css({ "color": "#cc0202" });
    $(this).nextAll().css({ "color": "#cc0202" });
});

$(document).ready(function () {
    var availableTags = [
        "ActionScript",
        "AppleScript",
        "Asp",
        "BASIC",
        "C",
        "C++",
        "Clojure",
        "COBOL",
        "ColdFusion",
        "Erlang",
        "Fortran",
        "Groovy",
        "Haskell",
        "Java",
        "JavaScript",
        "Lisp",
        "Perl",
        "PHP",
        "Python",
        "Ruby",
        "Scala",
        "Scheme"
    ];
    $("#search-header").autocomplete({
        source: "/restaurants/searchquery"
    });

    $("#autodetect").on('click', function () {
        getLocation();
        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition);
            } else {
                x.innerHTML = "Geolocation is not supported by this browser.";
            }
        }
        function showPosition(position) {
            var positions = `${position.coords.latitude} ${position.coords.longitude}`;
            $.ajax({
                url: `/SetUserLocation/${position.coords.latitude}/${position.coords.longitude}`,
                type: 'Get',
                success: function () {
                    window.location = "/home";
                }
            });
        }
    });
});