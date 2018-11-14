var tmp6;
var colspan; var tmp_id; var subject_id; var subject_name; var subject_credit; var subject_number; var subject_hour;
var tmp1; var tmp2; var tmp3; var tmp4; var tmp5; var tmp6; var j; var k; var l; var a; var i; var value; var valdate; var tablecellcheck; var colspanvalue;
var date2 = ["M", "T", "W", "H", "F", "S"];
// หน้า TimeSchedule //
$("#DDL_DEPARTMENT").change(function () {
    $("#Count").val(1);
    $("#BRANCH_FORM").submit();
});
$("#DDL_SEMESTERYEAR").change(function () {
    $("#BRANCH_FORM").submit();
});
$("#DDL_BRANCH").change(function () {
    $("#BRANCH_FORM").submit();
});

function drag_drop() {

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
            tmp1 = 21 - (parseInt(colspan) / 4) + 1;

            for (valdate = 0; valdate < 6; valdate++) {
                for (i = 21; i > tmp1; i--) {
                    tmp2 = date2[valdate] + "id_" + i;
                    $("#" + tmp2).droppable("disable");
                }
            }
            for (i = 0; i < 6; i++) {
                for (l = 8; l <= 21; l++) {
                    tablecellcheck = $("#" + date2[i] + "id_" + l).children().children("p").text().trim();
                    if (tablecellcheck != "") {
                        var cols = $("#" + date2[i] + "id_" + l).attr("colspan");
                        var stun2 = l - (parseInt(colspan) / 4) + 1;
                        for (k = stun2; k <= l; k++) {
                            $("#" + date2[i] + "id_" + k).droppable("disable");
                        }
                    }
                }
            }
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
            tmp1 = 21 - (parseInt(colspan) / 4) + 1;

            for (i = 0; i < 6; i++) {
                for (l = 8; l <= 21; l++) {
                    $("#" + date2[i] + "id_" + l).droppable("enable");
                }
            }

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
            for (i = 0; i < 6; i++) {
                for (l = 8; l <= 21; l++) {
                    $("#" + date2[i] + "id_" + l).css("background-color", "");
                }
            }
        }
        ,
        over: function (event, elem) {
            tmp_id = $(this).attr("id");
            $(this).addClass("over");
            if (tmp_id != null) {
                tmp1 = tmp_id.split("_");
                value = (parseInt(tmp1[1]) - 1) + (parseInt(colspan) / 4);
            }
            for (i = parseInt(tmp1[1]); i <= value; i++) {
                tmp2 = tmp1[0] + "_" + i;
                $("#" + tmp2).css("background-color", "#d6d9db");
            }
            console.log("over");
        }
        ,
        out: function (event, elem) {
            tmp_id = $(this).attr("id");
            if (tmp_id != null) {
                tmp1 = tmp_id.split("_");
                value = (parseInt(tmp1[1]) - 1) + (parseInt(colspan) / 4);
            }
            for (i = parseInt(tmp1[1]); i <= value; i++) {
                tmp2 = tmp1[0] + "_" + i;
                $("#" + tmp2).css("background-color", "");
            }
            $(this).removeClass("over");
        }
    });

    $(".x_button").click(function () {
        var rr = $(this).val();
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
drag_drop();
    $("#TableLocation tbody tr").click(function () {
        if ($(this).find("input").is(':checked')) {
            $(this).css("background-color", "");
            $(this).find("input").prop('checked', false);
        }
        else {
            var checkbool = false;
            subject_id = $(this).find("#subject_id").val();
            subject_name = $(this).find("#subject_name").val();
            subject_credit = $(this).find("#subject_credit").val();
            subject_number = $(this).find("#subject_number").val();
            subject_hour = $(this).find("#subject_hour").val();
            var s = subject_id + " " + subject_name;
            $("#gate").val(checkbool);
            for (i = 0; i < 6; i++) {
                for (j = 8; j <= 21; j++) {
                    tablecellcheck = $("#" + date2[i] + "id_" + j).children().children("p").text().trim();
                    if (tablecellcheck == s) {
                        checkbool = true;
                    }
                }
            }
            if (checkbool == false) {
                $("#TableLocation tbody tr").find("input").prop('checked', false);
                $("#TableLocation tbody tr").css("background-color", "");

                $(this).css("background-color", "#d6d9db");
                $(this).find("input").prop('checked', true);
            }
            else {
                $("#TableLocation tbody tr").find("input").prop('checked', false);
                $("#TableLocation tbody tr").css("background-color", "");
            }
            $("#gate2").val(checkbool);
        }
    });
$("#TableLocation2").click(function () {
    if ($("#TableLocation tbody tr").find("input").is(':checked')) {
        $("#TableLocation tbody tr").find("input").prop('checked', false);
        $("#TableLocation tbody tr").css("background-color", "");
        colspan = 4 * subject_hour;
        colspanvalue = (parseInt(tmp1[1]) - 1) + parseInt(subject_hour);
        var checkbool = false;
        for (i = parseInt(tmp1[1]); i <= colspanvalue; i++) {
            var aa = tmp1[0] + "_" + i;
            tablecellcheck = $("#" + aa).html().trim();
            if (tablecellcheck != "") {
                checkbool = true;
            }
        }
        tablecellcheck = $("#" + tmp_id).html().trim();
        if (tablecellcheck == "" && checkbool == false) {
            $("#" + tmp_id).attr('colspan', colspan);
            if (subject_credit == "3(3-0-6)") {
                $("#" + tmp_id).html('<div class="" style="background-color:#25b0ee;width:100%;height:50px"><div id="x_button" class="btn x_button btn-default pull-right text-center">X</div><p>' + subject_id + ' ' + subject_name + '</p></div>');
            }
            else {
                $("#" + tmp_id).html('<div class="" style="background-color:#D3D3D3;width:100%;height:50px"><div id="x_button" class="btn x_button btn-default pull-right text-center">X</div><p>' + subject_id + ' ' + subject_name + '</p></div>');
            }
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
            //Table1();
            drag_drop();
        }
    }
});
$("#TableLocation2 tr td div").click(function () {
    tmp_id = $(this).parent().attr("id");
    $("#gate3").val(tmp_id);
});
$("#TableLocation2 tr td").hover(function () {
    tmp_id = $(this).attr("id");
    if (tmp_id != null) {
        tmp1 = tmp_id.split("_");
        tmp2 = parseInt(tmp1[1]) - 1;
        tmp3 = tmp1[0] + "_" + tmp2;
        tmp4 = parseInt(tmp1[1]) + 1;
        tmp5 = tmp1[0] + "_" + tmp4;
        colspanvalue = (parseInt(tmp1[1]) - 1) + parseInt(subject_hour);
        if ($("#TableLocation tbody tr").find("input").is(':checked')) {
            for (value = parseInt(tmp1[1]); value <= colspanvalue; value++) {
                var aa = tmp1[0] + "_" + value;
                $("#" + aa).css("background-color", "#d6d9db");
            }
        }
    }
}, function () {
    tmp_id = $(this).attr("id");
    if (tmp_id != null) {
        tmp1 = tmp_id.split("_");
        tmp2 = parseInt(tmp1[1]) - 1;
        tmp3 = tmp1[0] + "_" + tmp2;
        tmp4 = parseInt(tmp1[1]) + 1;
        tmp5 = tmp1[0] + "_" + tmp4;

        colspanvalue = (parseInt(tmp1[1]) - 1) + parseInt(subject_hour);
        for (value = parseInt(tmp1[1]); value <= colspanvalue; value++) {
            var aa = tmp1[0] + "_" + value;
            $("#" + aa).css("background-color", "");
        }
    }
});
//-----------------------------------------------------------//

// หน้า อาคารเรียน/ห้องเรียน //
$("#DDL_BUILDING").change(function () {
    $("#BUILDING_FORM").submit();
});
$("#DDL_DATE").change(function () {
    $("#BUILDING_FORM").submit();
});
//-----------------------------------------------------------//

// หน้า ครูผู้สอน //
$("#DDL_PROFESSOR").change(function () {
    $("#PROFESSOR_FORM").submit();
});
//-----------------------------------------------------------//

// หน้าข้อมูล //
//$('#DataTable').DataTable({
//    "pagingType": "full_numbers",
//    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
//});
//-----------------------------------------------------------//