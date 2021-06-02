$(function () {

    //var MealTypebucket = getQueryVariable("MealType");
    debugger;
    var Location = getQueryVariable("Location");
    var Query = getQueryVariable("Query");
    var Deals = getQueryVariable("Deals");
    var Tags = getQueryVariable("Tags");
    var PageNumber = getQueryVariable("PageNumber");
    var RowsOfPage = getQueryVariable("RowsOfPage");
    var SortingCol = getQueryVariable("SortingCol");
    var SortType = getQueryVariable("SortType");
    var FoodNaturebucket = getQueryVariable("FoodNature");
    var Cuisinebucket = getQueryVariable("Cuisine");
    var RestaurantTypebucket = getQueryVariable("RestaurantType");
    var Guest = getQueryVariable("Guest");
    var Area = getQueryVariable("Area");
    var Date = getQueryVariable("Date");


    //checkedvaluesfrombucket(MealTypebucket);
    checkedvaluesfrombucket(FoodNaturebucket);
    checkedvaluesfrombucket(Cuisinebucket);
    checkedvaluesfrombucket(RestaurantTypebucket);
    addTokenOnScreen(Query);
    addTokenOnScreen(Deals);
    addTokenOnScreen(Tags);

    if (SortingCol != undefined && SortingCol != null && SortingCol != '') {
        var col = parseInt(SortingCol);
        $(".sort-col").val(col);
    }

    if (Date != undefined && Date != null && Date != '') {
        $("#search-query-date").val(Date);
    }

    if (Guest != undefined && Guest != null && Guest != '') {
        var col = parseInt(Guest);
        $("#search-query-guest").val(col);
    }

    if (Area != undefined && Area != null && Area != '') {
        var col = parseInt(Area);
        $("#search-query-seating").val(col);
    }

    $("#search-query-date").on('change', function () {
        Date = $(this).val();
        reRunQuery();
    });

    $("#search-query-guest").on('change', function () {
        Guest = $(this).val();
        reRunQuery();
    });

    $(".sort-col").on('change', function () {
        SortingCol = $(this).val();
        reRunQuery();
    });

    $("#search-query-seating").on('change', function () {
        Area = $(this).val();
        reRunQuery();
    });

    function reRunQuery() {
        showLoader();
        try {
            var newquerystring = `?Location=${Location}&Query=${Query}&Deals=${Deals}&Tags=${Tags}&PageNumber=${PageNumber}&RowsOfPage=${RowsOfPage}&SortingCol=${SortingCol}&SortType=${SortType}&Cuisine=${Cuisinebucket}&FoodNature=${FoodNaturebucket}&RestaurantType=${RestaurantTypebucket}&Guest=${Guest}&Date=${Date}&Area=${Area}`;
            var currenturi = window.location.href.split('?')[0];
            location.href = currenturi + newquerystring;
        } catch (err) {
            hideLoader();
        }
    }

    $(".filter-item").on('change', function () {
        //MealTypebucket = getQueryVariable("MealType");
        //MealTypebucket = MealTypebucket == undefined ? '' : MealTypebucket;
        // FoodNaturebucket = getQueryVariable("FoodNature");
        showLoader();
        try {
            reFillQuery();
            FoodNaturebucket = FoodNaturebucket == undefined ? '' : FoodNaturebucket;
            //RestaurantTypebucket = getQueryVariable("RestaurantType");
            RestaurantTypebucket = RestaurantTypebucket == undefined ? '' : RestaurantTypebucket;
            //Cuisinebucket = getQueryVariable("Cuisine");
            Cuisinebucket = Cuisinebucket == undefined ? '' : Cuisinebucket;
            var datatype = $(this).attr('data-type');
            var label = $(this).attr('data-value');
            if (this.checked) {
                switch (datatype) {
                    case "Cuisine":
                        Cuisinebucket = Cuisinebucket == undefined || Cuisinebucket == '' ? label : Cuisinebucket + "," + label;
                        break;
                    case "FoodNature":
                        FoodNaturebucket = FoodNaturebucket == undefined || FoodNaturebucket == '' ? label : FoodNaturebucket + "," + label;
                        break;
                    case "RestaurantType":
                        RestaurantTypebucket = RestaurantTypebucket == undefined || RestaurantTypebucket == '' ? label : RestaurantTypebucket + "," + label;
                        break;
                }
            }

            var newquerystring = `?Location=${Location}&Query=${Query}&Deals=${Deals}&Tags=${Tags}&PageNumber=${PageNumber}&RowsOfPage=${RowsOfPage}&SortingCol=${SortingCol}&SortType=${SortType}&Cuisine=${Cuisinebucket}&FoodNature=${FoodNaturebucket}&RestaurantType=${RestaurantTypebucket}&Guest=${Guest}&Date=${Date}&Area=${Area}`;
            var currenturi = window.location.href.split('?')[0];
            location.href = currenturi + newquerystring;
        }
        catch (err) {
            hideLoader();
        }

    });

    function reFillQuery() {
        Location = getQueryVariable("Location");
        Query = getQueryVariable("Query");
        Deals = getQueryVariable("Deals");
        Tags = getQueryVariable("Tags");
        PageNumber = getQueryVariable("PageNumber");
        RowsOfPage = getQueryVariable("RowsOfPage");
        SortingCol = getQueryVariable("SortingCol");
        SortType = getQueryVariable("SortType");
        FoodNaturebucket = getQueryVariable("FoodNature");
        Cuisinebucket = getQueryVariable("Cuisine");
        RestaurantTypebucket = getQueryVariable("RestaurantType");
        Guest = getQueryVariable("Guest");
        Date = getQueryVariable("Date");
        Area = getQueryVariable("Area");
    }

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }


    function checkedvaluesfrombucket(bucket) {
        if (bucket !== undefined && bucket !== '') {
            var bucketArray = bucket.split(",");
            checkenableforarray(bucketArray);
        }
    }

    idleTimer = null;
    idleState = false;
    idleWait = 2000;
    $(".remove-token").on('click', function () {
        showLoader();
        try {
            var text = $(this).text();
            $(this).remove();
            $(`input[data-value="${text}"]`).attr('checked', false); // Checks it.attr('data-value');
            var currenturi = decodeURIComponent(window.location.href);
            currenturi = currenturi.replace(text + ",", '');
            currenturi = currenturi.replace(text, '');
            currenturi = currenturi.replace(' ,', '');
            currenturi = currenturi.replace(',,', '');
            location.href = currenturi;
        } catch (err) {
            hideLoader();
        }
    });


    function checkenableforarray(array) {
        $(".tag-container").css("display", "flex");
        $.each(array, function (i) {
            if (array !== '') {
                $(`input[data-value="${array[i]}"]`).attr('checked', true);
                addTokenOnScreen(array[i]);
            }
        });
    }

    function addTokenOnScreen(item) {
        if (item != null && item != undefined && item != '') {
            $(".tag-container").css("display", "flex");
            $("#tokens").append(`<p class="tag text-capitalize remove-token">${item}</p>`);
        }
    }

    var NoMoreData = false;
    var inProgress = false;

    $(window).scroll(function () {
        if ((parseInt($(window).scrollTop()) - 600) == (($(document).height() -
            $(window).height()) - 600) && !NoMoreData && !inProgress) {

            inProgress = true;
            var localpagenumber = parseInt(PageNumber) + 1;
            //  $("#loadingDiv").show();
            var newquerystring = `?Location=${Location}&Query=${Query}&Deals=${Deals}&Tags=${Tags}&PageNumber=${localpagenumber}&RowsOfPage=${RowsOfPage}&SortingCol=${SortingCol}&SortType=${SortType}&Cuisine=${Cuisinebucket}&FoodNature=${FoodNaturebucket}&RestaurantType=${RestaurantTypebucket}&Guest=${Guest}&Date=${Date}&Area=${Area}`;
            showLoader();
            if (localpagenumber > 1) {
                $.ajax({
                    url: `/restaurants/searchpartial${newquerystring}`,
                    type: 'Get',
                    success: function (data) {
                        if (data == undefined || data == null)
                            NoMoreData = true;
                        $("#restaurant-filter-partial").append(data);
                        //  $("#loadingDiv").hide();
                        hideLoader();
                    },
                    error: function (err) {
                        hideLoader();
                    }

                });
            } else {
                hideLoader();
            }
        }
    });

    //function showPosition(position) {
    //    var latlon = position.coords.latitude + "," + position.coords.longitude;

    //    var img_url = `https://maps.googleapis.com/maps/api/staticmap?center=${latlon}&zoom=14&size=400x300&sensor=false&key=YOUR_KEY`;

    //    document.getElementById("mapholder").innerHTML = "<img src='" + img_url + "'>";

    //}




});

