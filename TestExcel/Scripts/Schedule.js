var tmp6;
var colspan; var tmp_id; var subject_id; var subject_name; var subject_credit; var subject_number; var subject_hour;
var tmp1; var tmp2; var tmp3; var tmp4; var tmp5; var tmp6; var j; var k; var l; var a; var i; var value; var valdate; var tablecellcheck; var colspanvalue;
var date2 = ["M", "T", "W", "H", "F", "S"];
// หน้า TimeSchedule //
$("#DDL_DEPARTMENT").change(function () {
    $("#Count").val(1);
    $("#BRANCH_FORM").submit();
});
$("#ddl_Year").change(function () {
    $("#BRANCH_FORM").submit();
});
$("#ddl_Semester").change(function () {
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
                    if (tablecellcheck !== "") {
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
            if (tmp_id !== null) {
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
            if (tmp_id !== null) {
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
                    if (tablecellcheck === s) {
                        checkbool = true;
                    }
                }
            }
            if (checkbool === false) {
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
            if (tablecellcheck !== "") {
                checkbool = true;
            }
        }
        tablecellcheck = $("#" + tmp_id).html().trim();
        if (tablecellcheck === "" && checkbool === false) {
            $("#" + tmp_id).attr('colspan', colspan);
            if (subject_credit === "3(3-0-6)") {
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
$('#FIRST_SAVE_TIMESTART').on("change", function () {
    var TimeStart = $('#FIRST_SAVE_TIMESTART').val();
    var optionlisting = "";
    $('#FIRST_SAVE_TIMEEND option').remove();
    for (i = parseInt(TimeStart) +1; i <= 21; i++) {
        optionlisting = optionlisting + '<option value="'+ i +'">' + i + ":00" + '</option>';
    }
    $('#FIRST_SAVE_TIMEEND').html(optionlisting);
});
$('#SECOND_SAVE_TIMESTART').on("change", function () {
    var TimeStart = $('#SECOND_SAVE_TIMESTART').val();
    var optionlisting = "";
    $('#SECOND_SAVE_TIMEEND option').remove();
    for (i = parseInt(TimeStart) + 1; i <= 21; i++) {
        optionlisting = optionlisting + '<option value="' + i + '">' + i + ":00" + '</option>';
    }
    $('#SECOND_SAVE_TIMEEND').html(optionlisting);
});
$("#TableLocation2 tr td div").click(function () {
    var optionlisting = "";
    var TimeStart, tmp_id2, tmp12, first_id, first_subjectid, first_name, first_number, first_branch, first_professor, first_timestart, first_timeend, first_date;
    var second_id, second_subjectid, second_name, second_number, second_branch, second_professor, second_timestart, second_timeend, second_date;
    if ($(this).find("#Trigger").val() === 1) {
            tmp_id2 = $(this).parent().attr("id");
            tmp12 = tmp_id2.split("id_");
            first_id = $(this).find("#" + tmp12[0] + "id_" + tmp12[1]).val();
            first_subjectid = $(this).find("#" + tmp12[0] + "subjectid_" + tmp12[1]).val();
            first_name = $(this).find("#" + tmp12[0] + "name_" + tmp12[1]).val();
            first_number = $(this).find("#" + tmp12[0] + "number_" + tmp12[1]).val();
            first_branch = $(this).find("#" + tmp12[0] + "branch_" + tmp12[1]).val();
            first_professor = $(this).find("#" + tmp12[0] + "professor_" + tmp12[1]).val();
            first_timestart = $(this).find("#" + tmp12[0] + "timestart_" + tmp12[1]).val();
            first_timeend = $(this).find("#" + tmp12[0] + "timeend_" + tmp12[1]).val();
            first_date = $(this).find("#" + tmp12[0] + "date_" + tmp12[1]).val();
            $("#First_Header").html(first_subjectid + " " + first_name);
            $("#FIRST_SECTION_ID").val(first_id);
            $("#FIRST_SAVE_NUMBER").val(first_number);
            $("#FIRST_SAVE_DATE").val(first_date);
            $("#FIRST_SAVE_TIMESTART").val(first_timestart);
            TimeStart = $('#FIRST_SAVE_TIMESTART').val();
            $('#FIRST_SAVE_TIMEEND option').remove();
            for (i = parseInt(TimeStart) + 1; i <= 21; i++) {
                optionlisting = optionlisting + '<option value="' + i + '">' + i + ":00" + '</option>';
            }
            $('#FIRST_SAVE_TIMEEND').html(optionlisting);
            $("#FIRST_SAVE_TIMEEND").val(first_timeend);
            $("#FIRST_SAVE_PROFESSOR").val(first_professor);
            $("#FIRST_SAVE_BRANCH").val(first_branch);
        }
        else {
            tmp_id2 = $(this).parent().attr("id");
            tmp12 = tmp_id2.split("id_");
            first_id = $(this).find("#First_" + tmp12[0] + "id_" + tmp12[1]).val();
            first_subjectid = $(this).find("#First_" + tmp12[0] + "subjectid_" + tmp12[1]).val();
            first_name = $(this).find("#First_" + tmp12[0] + "name_" + tmp12[1]).val();
            first_number = $(this).find("#First_" + tmp12[0] + "number_" + tmp12[1]).val();
            first_branch = $(this).find("#First_" + tmp12[0] + "branch_" + tmp12[1]).val();
            first_professor = $(this).find("#First_" + tmp12[0] + "professor_" + tmp12[1]).val();
            first_timestart = $(this).find("#First_" + tmp12[0] + "timestart_" + tmp12[1]).val();
            first_timeend = $(this).find("#First_" + tmp12[0] + "timeend_" + tmp12[1]).val();
            first_date = $(this).find("#First_" + tmp12[0] + "date_" + tmp12[1]).val();

            second_id = $(this).find("#Second_" + tmp12[0] + "id_" + tmp12[1]).val();
            second_subjectid = $(this).find("#Second_" + tmp12[0] + "subjectid_" + tmp12[1]).val();
            second_name = $(this).find("#Second_" + tmp12[0] + "name_" + tmp12[1]).val();
            second_number = $(this).find("#Second_" + tmp12[0] + "number_" + tmp12[1]).val();
            second_branch = $(this).find("#Second_" + tmp12[0] + "branch_" + tmp12[1]).val();
            second_professor = $(this).find("#Second_" + tmp12[0] + "professor_" + tmp12[1]).val();
            second_timestart = $(this).find("#Second_" + tmp12[0] + "timestart_" + tmp12[1]).val();
            second_timeend = $(this).find("#Second_" + tmp12[0] + "timeend_" + tmp12[1]).val();
            second_date = $(this).find("#Second_" + tmp12[0] + "date_" + tmp12[1]).val();

            $("#First_Header").html(first_subjectid + " " + first_name);
            $("#FIRST_SECTION_ID").val(first_id);
            $("#FIRST_SAVE_NUMBER").val(first_number);
            $("#FIRST_SAVE_DATE").val(first_date);
            $("#FIRST_SAVE_TIMESTART").val(first_timestart);
            TimeStart = $('#FIRST_SAVE_TIMESTART').val();
            $('#FIRST_SAVE_TIMEEND option').remove();
            for (i = parseInt(TimeStart) + 1; i <= 21; i++) {
                optionlisting = optionlisting + '<option value="' + i + '">' + i + ":00" + '</option>';
            }
            $('#FIRST_SAVE_TIMEEND').html(optionlisting);
            $("#FIRST_SAVE_TIMEEND").val(first_timeend);
            $("#FIRST_SAVE_PROFESSOR").val(first_professor);
            $("#FIRST_SAVE_BRANCH").val(first_branch);

            $("#Second_Header").html(second_subjectid + " " + second_name);
            $("#SECOND_SECTION_ID").val(second_id);
            $("#SECOND_SAVE_NUMBER").val(second_number);
            $("#SECOND_SAVE_DATE").val(second_date);
            $("#SECOND_SAVE_TIMESTART").val(second_timestart);
            TimeStart = $('#SECOND_SAVE_TIMESTART').val();
            $('#SECOND_SAVE_TIMEEND option').remove();
            for (i = parseInt(TimeStart) + 1; i <= 21; i++) {
                optionlisting = optionlisting + '<option value="' + i + '">' + i + ":00" + '</option>';
            }
            $('#SECOND_SAVE_TIMEEND').html(optionlisting);
            $("#SECOND_SAVE_TIMEEND").val(second_timeend);
            $("#SECOND_SAVE_PROFESSOR").val(second_professor);
            $("#SECOND_SAVE_BRANCH").val(second_branch);
        }
});
$("#TableLocation2 tr td").hover(function () {
    tmp_id = $(this).attr("id");
    if (tmp_id !== null) {
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
    if (tmp_id !== null) {
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