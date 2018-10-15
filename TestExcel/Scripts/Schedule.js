var tmp6;
var colspan; var tmp_id; var subject_id; var subject_name; var subject_credit; var subject_number; var subject_hour;
var tmp1; var tmp2; var tmp3; var tmp4; var tmp5; var tmp6; var j; var a; var i;
var date2 = ["M", "T", "W", "H", "F", "S"];
// หน้า TimeSchedule //
$("#DDL_DEPARTMENT").change(function () {
    $("#Count").val(1);
    $("#BRANCH_FORM").submit();
});
$("#DDL_BRANCH").change(function () {
    $("#BRANCH_FORM").submit();
});
$(".draggable").draggable({
    cursor: "move",
    revert: "invalid",
    start: function (event, ui) {
        console.log("start");
        colspan = $(this).parent().attr("colspan");
        $(this).parent().attr("colspan", "4");
        var tmp = $(this).parent().attr("id");
        tmp = tmp.split("_");
        a = parseInt(tmp[1]);
        j = (parseInt(colspan) / 4) + a;
        for (i = a; i < j; i++) {
            tmp6 = tmp[0] + "_" + i;
            $("#" + tmp6).show();
        }
    },
    stop: function (event, ui) {
        console.log("stop");
        $(this).parent().attr("colspan", colspan);

        var tmp = $(this).parent().attr("id");
        tmp = tmp.split("_");
        a = parseInt(tmp[1]);
        j = (parseInt(colspan) / 4) + a;
        a = a + 1;
        for (i = a; i < j; i++) {
            tmp6 = tmp[0] + "_" + i;
            $("#" + tmp6).hide();
        }

    }
});

$(".droppable").droppable({
    accept: ".draggable",
    drop: function (event, ui) {
        console.log("drop");
        $(this).removeClass("border").removeClass("over");
        var dropped = ui.draggable;
        var droppedOn = $(this);
        $(dropped).detach().css({ top: 0, left: 0 }).appendTo(droppedOn);
        $(this).css("background-color", "");
    },
    over: function (event, elem) {
        $(this).addClass("over");
        $(this).css("background-color", "#d6d9db");
        console.log("over");
    }
    ,
    out: function (event, elem) {
        $(this).removeClass("over");
        $(this).css("background-color", "");
    }
});
$(".x_button").click(function () {
    var rr = $(this).val();
    $("#gate").val(rr);
    $("#" + tmp_id).children().remove();

    var tmp_num = $("#" + tmp_id).attr("colspan");
    a = parseInt(tmp1[1]);
    j = (parseInt(tmp_num) / 4) + a;
    for (i = a; i < j; i++) {
        tmp6 = tmp1[0] + "_" + i;
        $("#" + tmp6).show();
    }

    $("#" + tmp_id).attr("colspan", "4");

});
$("#TableLocation tbody tr").click(function () {
    if ($(this).find("input").is(':checked')) {
        $(this).css("background-color", "");
        $(this).find("input").prop('checked', false);
    }
    else {
        $("#TableLocation tbody tr").find("input").prop('checked', false);
        $("#TableLocation tbody tr").css("background-color", "");

        $(this).css("background-color", "#d6d9db");
        $(this).find("input").prop('checked', true);
        subject_id = $(this).find("#subject_id").val();
        subject_name = $(this).find("#subject_name").val();
        subject_credit = $(this).find("#subject_credit").val();
        subject_number = $(this).find("#subject_number").val();
        subject_hour = $(this).find("#subject_hour").val();
    }
});
$("#TableLocation2").click(function () {
    if ($("#TableLocation tbody tr").find("input").is(':checked')) {
        $("#TableLocation tbody tr").find("input").prop('checked', false);
        $("#TableLocation tbody tr").css("background-color", "");

        colspan = 4 * subject_hour;
        $("#" + tmp_id).attr('colspan', colspan);
        $("#" + tmp_id).html('<div class="" style="background-color:#D3D3D3;width:100%;height:50px"><div id="x_button" class="btn x_button btn-default pull-right text-center">X</div> ' + subject_id + '  ' + subject_name + '</div>');
        $("#" + tmp_id).children().addClass("draggable");

        var tmp = $("#" + tmp_id).attr("id");
        tmp = tmp.split("_");
        a = parseInt(tmp[1]);
        j = (parseInt(colspan) / 4) + a;
        a = a + 1;
        for (i = a; i < j; i++) {
            tmp6 = tmp[0] + "_" + i;
            $("#" + tmp6).hide();
        }

        $(".draggable").draggable({
            cursor: "move",
            revert: "invalid",
            start: function (event, ui) {
                console.log("start");
                colspan = $(this).parent().attr("colspan");
                $(this).parent().attr("colspan", "4");
                var tmp = $(this).parent().attr("id");
                tmp = tmp.split("_");
                a = parseInt(tmp[1]);
                j = (parseInt(colspan) / 4) + a;
                for (i = a; i < j; i++) {
                    tmp6 = tmp[0] + "_" + i;
                    $("#" + tmp6).show();
                }
            },
            stop: function (event, ui) {
                console.log("stop");
                $(this).parent().attr("colspan", colspan);

                var tmp = $(this).parent().attr("id");
                tmp = tmp.split("_");
                a = parseInt(tmp[1]);
                j = (parseInt(colspan) / 4) + a;
                a = a + 1;
                for (i = a; i < j; i++) {
                    tmp6 = tmp[0] + "_" + i;
                    $("#" + tmp6).hide();
                }

            }
        });

        $(".x_button").click(function () {
            var rr = $(this).val();
            $("#gate").val(rr);
            $("#" + tmp_id).children().remove();

            var tmp_num = $("#" + tmp_id).attr("colspan");
            a = parseInt(tmp1[1]);
            j = (parseInt(tmp_num) / 4) + a;
            for (i = a; i < j; i++) {
                tmp6 = tmp1[0] + "_" + i;
                $("#" + tmp6).show();
            }

            $("#" + tmp_id).attr("colspan", "4");

        });
    }
});
$("#TableLocation2 tr td").hover(function () {
    tmp_id = $(this).attr("id");
    if (tmp_id != null) {
        tmp1 = tmp_id.split("_");
        tmp2 = parseInt(tmp1[1]) - 1;
        tmp3 = tmp1[0] + "_" + tmp2;
        tmp4 = parseInt(tmp1[1]) + 1;
        tmp5 = tmp1[0] + "_" + tmp4;
    }
    if ($("#TableLocation tbody tr").find("input").is(':checked')) {
        $("#" + tmp_id).css("background-color", "#d6d9db");
    }
    $("#gate").val(tmp_id);
}, function () {
    va = $(this).attr("id");
    $("#" + tmp_id).css("background-color", "");
    });
//-----------------------------------------------------------//

// หน้า อาคารเรียน/ห้องเรียน //
$("#DDL_BUILDING").change(function () {
    $("#Count").val(1);
    $("#BUILDING_FORM").submit();
});
$("#DDL_CLASSROOM").change(function () {
    $("#BUILDING_FORM").submit();
});
//-----------------------------------------------------------//