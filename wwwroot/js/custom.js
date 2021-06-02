$(function () {
    $('#singleupload input.files').fileuploader({
        limit: 1,
        addMore: false,
    });
    $('.multiple-uploads input.files').fileuploader({
        limit: 20,
        maxSize: 50,
        addMore: true
    })
    $(".custom-delete-img").click(function () {
        $(this).parents(".fileuploader-items").remove();
    });
    $(".delete-record").click(function () {
        var str = $(this).attr("href");
        $(".delete-button-record").attr("href", str);
    });
    $("#toggleButton").click(function () {
        if ($("#sidebarHidden .sidebar").hasClass("w-0")) {
            $("#sidebarHidden .sidebar").removeClass("w-0");
            $("#sidebarHidden .sidebar").addClass("w-250");
        } else {
            $("#sidebarHidden .sidebar").removeClass("w-250");
            $("#sidebarHidden .sidebar").addClass("w-0");
        }
    });

    $(".content-wrapper").click(function () {
        if ($("#sidebarHidden .sidebar").hasClass("w-250")) {
            $("#sidebarHidden .sidebar").addClass("w-0");
            $("#sidebarHidden .sidebar").removeClass("w-250");
        }
    });
    if ($(window).width() < 992) {
        $("#mobile-col-3").css("display", "none");
        $("#mobile-col-9").removeClass("col-md-9");
        $("#mobile-col-9").addClass("col-md-12");
    }

    if ($(window).width() < 576) {
        $("#mobile-col-3").css("display", "none");
        $("#mobile-col-9").addClass("col-md-9");
        $("#mobile-col-9").removeClass("col-md-12");
    }

    if ($(window).width() < 400) {
        $(".card .mobile-dash").css("padding", "1.875rem 0.8rem");
    }

    /*$(document).on('click', '.prev', function () {
            //1- get first element to check if it has class 'active', 
            // to prevent class 'active' from moving to prev button on click,
            // if explanation isn't clear try removing if(){} to see it.
            const first = $(this).siblings().first();
            if (!first.hasClass('active')) {
                //2- search <li>'s to get the one that has 'active' class.
                const active = $(this).siblings('.active');
                //3- get the previous item of the <li class"active"> to move to.
                const prevItem = active.prev();
                //4- get href of the item to redirect to.
                const link = prevItem.children('a').prop('href');
                //5- remove 'active' class from the current <li> and give it to prev <li>.
                active.removeClass('active');
                prevItem.addClass('active');
                //6- redirect to href of the new <li>.
                window.location.href = link;
            }
        });
        $(document).on('click', '.next', function () {
            const last = $(this).siblings().last();
            if (!last.hasClass('active')) {
                const active = $(this).siblings('.active');
                const nextItem = active.next();
                const link = nextItem.children('a').prop('href');
                active.removeClass('active');
                nextItem.addClass('active');
                window.location.href = link;
            }*/

});

$(document).ready(function () {
    //$(".tags").each(function () {
    //    let hashtag, hashtagArray, container, t, hdnval;
    //    hashtag = $(this);
    //    container = $(`#${hashtag.attr("data-target-container")}`);
    //    hdnval = $(`#${hashtag.attr('data-target')}`);
    //    hashtagArray = [];
    //    hashtag.on('keyup', () => {
    //        if (event.which === 32 && hashtag.val().length > 1) {
    //            var text = $(document.createTextNode(hashtag.val()));
    //            var p = $(document.createElement('p'));
    //            container.append(p);
    //            p.append(text);
    //            p.addClass('tag');
    //            p.attr("asp-for", "Tags");
    //            hashtag.val('');
    //            var tags = '';
    //            tagsvalues = $(`#${hashtag.attr("data-target-container")} p`).each(function () {
    //                tags += $(this).text().trim() + ",";
    //            });
    //            hdnval.val(tags);
    //            let deleteTags = $('.tag');
    //            for (let i = 0; i < deleteTags.length; i++) {
    //                deleteTags[i].addEventListener('click', () => {
    //                    deleteTags[i].remove();
    //                    var newval = hdnval.val().split(/(\s+)/).filter(function (e) { return e.trim().length > 0; });
    //                    newval.splice(i, 1);
    //                    hdnval.val(newval);
    //                });
    //            }
    //        }
    //    });

    //    var newtag = hdnval.val().split(/(\s+)/).filter(function (e) { return e.trim().length > 0; });
    //    hdnval.val(newtag);
    //    let deleteTags = $('.tag');
    //    for (let i = 0; i < deleteTags.length; i++) {
    //        deleteTags[i].addEventListener('click', () => {
    //            deleteTags[i].remove();
    //            var tagss = hdnval.val().split(",").filter(function (e) { return e.trim().length > 0; });
    //            tagss.splice(i, 1);
    //            hdnval.val(tagss);
    //            console.log(tagss);
    //        });
    //    }
    //});

    $("#cusineadd").click(function () {
        var hdnval = $(".hdncusine").val();
        var hdntags = $("#hdncusine").val();
        if (hdnval && hdnval != "") {
            hdntags += "," + hdnval;
            $("#hdncusine").val(hdntags);

            var container = $("#hdncusinecontainer");
            var text = $(document.createTextNode(hdnval));
            var p = $(document.createElement('p'));
            container.append(p);
            p.append(text);
            p.addClass('tag tagcusine');
            /*p.attr("asp-for", "Tags");*/

            $(".hdncusine").val("");

            let deleteTags = $('.tagcusine');
            for (let i = 0; i < deleteTags.length; i++) {
                deleteTags[i].addEventListener('click', () => {
                    deleteTags[i].remove();
                    var output = [];
                    $("#hdncusinecontainer > p").each(function (index, item) {
                        output.push(item.innerText);
                    });
                    $("#hdncusine").val(output.join(","));
                });
            }
        }
    });

    $(".tagcusine").click(function () {
        let hdnval = $("#hdncusine");
        $(this).remove();
        var output = [];
        $("#hdncusinecontainer > p").each(function (index, item) {
            output.push(item.innerText);
        });
        hdnval.val(output.join(","));

        let deleteTags = $('.tagcusine');
        for (let i = 0; i < deleteTags.length; i++) {
            deleteTags[i].addEventListener('click', () => {
                deleteTags[i].remove();
                var output = [];
                $("#hdncusinecontainer > p").each(function (index, item) {
                    output.push(item.innerText);
                });
                $("#hdncusine").val(output.join(","));
            });
        }
    });

    $("#tagsadd").click(function () {
        var hdnval = $(".hdntags").val();
        var hdntags = $("#hdntags").val();
        if (hdnval && hdnval != "") {
            hdntags += "," + hdnval;
            $("#hdntags").val(hdntags);

            var container = $("#hdntagscontainer");
            var text = $(document.createTextNode(hdnval));
            var p = $(document.createElement('p'));
            container.append(p);
            p.append(text);
            p.addClass('tag tagtags');
            /*p.attr("asp-for", "Tags");*/

            $(".hdntags").val("");

            let deleteTags = $('.tagtags');
            for (let i = 0; i < deleteTags.length; i++) {
                deleteTags[i].addEventListener('click', () => {
                    deleteTags[i].remove();
                    var output = [];
                    $("#hdntagscontainer > p").each(function (index, item) {
                        output.push(item.innerText);
                    });
                    $("#hdntags").val(output.join(","));
                });
            }
        }
    });

    $(".tagtags").click(function () {
        let hdnval = $("#hdntags");
        $(this).remove();
        var output = [];
        $("#hdntagscontainer > p").each(function (index, item) {
            output.push(item.innerText);
        });
        hdnval.val(output.join(","));

        let deleteTags = $('.tagtags');
        for (let i = 0; i < deleteTags.length; i++) {
            deleteTags[i].addEventListener('click', () => {
                deleteTags[i].remove();
                var output = [];
                $("#hdntagscontainer > p").each(function (index, item) {
                    output.push(item.innerText);
                });
                $("#hdntags").val(output.join(","));
            });
        }
    });

    //$(".hdncusine").each(function () {
    //    let hashtag, hdnval;
    //    hashtag = $(this);
    //    hdnval = $(`#${hashtag.attr('data-target')}`);

    //    let deleteTags = $('.tagcusine');
    //    for (let i = 0; i < deleteTags.length; i++) {
    //        deleteTags[i].addEventListener('click', () => {
    //            deleteTags[i].remove();
    //            var tagss = hdnval.val().split(",").filter(function (e) { return e.trim().length > 0; });
    //            tagss.splice(i, 1);
    //            hdnval.val(tagss);
    //        });
    //    }
    //});

    //$(".hdntags").each(function () {
    //    let hashtag, hdnval;
    //    hashtag = $(this);
    //    hdnval = $(`#${hashtag.attr('data-target')}`);

    //    let deleteTags = $('.tagtags');
    //    for (let i = 0; i < deleteTags.length; i++) {
    //        deleteTags[i].addEventListener('click', () => {
    //            deleteTags[i].remove();
    //            var tagss = hdnval.val().split(",").filter(function (e) { return e.trim().length > 0; });
    //            tagss.splice(i, 1);
    //            hdnval.val(tagss);
    //        });
    //    }
    //});
});







