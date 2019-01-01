$(document).ready(function () {
    var colspan, tmp_id, tmp1, tmp2, tmp3, tmp4, tmp5, tmp6, tmp7, tmpdate, tmptimestart, tmptimeend, j, k, l, a, i, value, valdate, tablecellcheck, tablecellcheck2, tablecellcheck3, colspanvalue;
    var section_id, subject_classroom, subject_id, subject_name, subject_credit, subject_number, subject_hour, subject_timestart, subject_timeend, subject_professor, subject_branch, subject_date;
    var check_id, last_section_id, last_subject_classroom, last_subject_id, last_subject_name, last_subject_credit, last_subject_number, last_subject_hour, last_subject_timestart, last_subject_timeend, last_subject_professor, last_subject_branch, last_subject_date;
    var date2 = ["M", "T", "W", "H", "F", "S"];
    var hour,hour1,hour2;
    function drag_drop() {

        $(".draggable").draggable({
            cursor: "pointer",
            revert: "invalid",
            start: function (event, ui) {
                console.log("start");
                colspan = $(this).parent().attr("colspan");
                var checkhave = $(this).parent();
                var tmp = $(this).parent().attr("id");
                tmp = tmp.split("_");
                var checkhavefirst = $(this).parent().find(".getdata:first #searchId").val();
                var checkhavelast = $(this).parent().find(".getdata:last #searchId").val();
                if (checkhavefirst !== checkhavelast) {
                    check_id = checkhavelast;
                    subject_timestart = $(this).parent().find(".getdata:last #First_timestart_" + check_id).val();
                    subject_timeend = $(this).parent().find(".getdata:last #First_timeend_" + check_id).val();
                    hour = (parseInt(subject_timeend) - parseInt(subject_timestart)) * 4;
                    checkhave.attr("colspan", hour);
                    colspan = $(this).parent().attr("colspan");
                    a = parseInt(tmp[1]);
                    j = (parseInt(colspan) / 4) + a;
                }
                else {
                    $(this).parent().attr("colspan", "4");
                }
                check_id = $(this).find("#searchId").val();
                subject_timestart = $(this).find("#First_timestart_" + check_id).val();
                subject_timeend = $(this).find("#First_timeend_" + check_id).val();
                hour = parseInt(subject_timeend) - parseInt(subject_timestart);
                section_id = $(this).find("#Second_id_" + check_id).val();
                if (section_id !== "0" || section_id !== null) {
                    last_subject_timestart = $(this).find("#Second_timestart_" + check_id).val();
                    last_subject_timeend = $(this).find("#Second_timeend_" + check_id).val();
                    hour1 = parseInt(last_subject_timeend) - parseInt(last_subject_timestart);
                }
                a = parseInt(tmp[1]);
                j = (parseInt(colspan) / 4) + a;
                tmp1 = 21 - (parseInt(colspan) / 4) + 1;

                for (valdate = 0; valdate < 6; valdate++) {
                    for (i = 21; i >= tmp1; i--) {
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
                if (checkhavefirst !== checkhavelast) {
                    for (i = a + 1; i < j; i++) {
                        tmp6 = tmp[0] + "_" + i;
                        $("#" + tmp6).hide();
                    }
                } else {
                    for (i = a; i < j; i++) {
                        tmp6 = tmp[0] + "_" + i;
                        $("#" + tmp6).show();
                    }
                }
            },
            stop: function (event, ui) {

                console.log("stop");
                if (section_id == "0" || section_id == null) {
                    colspan = hour * 4;
                }
                else {
                    colspan = (hour + hour1) * 4;
                }
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
                hour = parseInt(hour) + parseInt(tmptimestart);
                $(this).find("#First_timestart_" + check_id).val(tmptimestart);
                $(this).find("#First_timeend_" + check_id).val(hour);
                $(this).find("#First_date_" + check_id).val(tmpdate);
                if (section_id !== "0" || section_id !== null) {
                    hour1 = hour + hour1;
                    $(this).find("#Second_timestart_" + check_id).val(hour);
                    $(this).find("#Second_timeend_" + check_id).val(hour1);
                    $(this).find("#Second_date_" + check_id).val(tmpdate);
                }
            }
        });
        $(".droppable").droppable({
            accept: ".draggable",
            drop: function (event, ui) {
                console.log("drop");
                tmp_id = $(this).attr("id");
                tmp7 = tmp_id.split("id_");
                tmpdate = tmp7[0];
                tmptimestart = parseInt(tmp7[1]);
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
            drag_drop();
        });
    }
    drag_drop();
   // function Table1() {
        $("#TableLocation tbody tr").click(function () {
            if ($(this).find(".tablecheckbox").is(':checked')) {
                $(this).css("background-color", "");
                $(this).find(".tablecheckbox").prop('checked', false);
            }
            else {
                var checkbool = false;
                section_id = $(this).find("#sub_id").val();
                subject_id = $(this).find("#subject_id").val();
                subject_name = $(this).find("#subject_name").val();
                subject_credit = $(this).find("#subject_credit").val();
                subject_number = $(this).find("#subject_number").val();
                subject_hour = $(this).find("#subject_hour").val();
                subject_timestart = $(this).find("#subject_timestart").val();
                subject_timeend = $(this).find("#subject_timeend").val();
                subject_date = $(this).find("#subject_date").val();
                subject_classroom = $(this).find("#subject_classroom").val();
                last_subject_id = "0";
                last_subject_name = "";
                last_subject_credit = "";
                last_subject_number = "";
                last_subject_hour = "";
                last_subject_timestart = "";
                last_subject_timeend = "";
                last_subject_date = "";

                last_section_id = $(this).find("#Last_sub_id").val();
                if (last_section_id != null) {
                    last_subject_id = $(this).find("#Last_subject_id").val();
                    last_subject_name = $(this).find("#Last_subject_name").val();
                    last_subject_credit = $(this).find("#Last_subject_credit").val();
                    last_subject_number = $(this).find("#Last_subject_number").val();
                    last_subject_timestart = $(this).find("#Last_subject_timestart").val();
                    last_subject_timeend = $(this).find("#Last_subject_timeend").val();
                    last_subject_date = $(this).find("#Last_subject_date").val();
                    last_subject_classroom = $(this).find("#Last_subject_classroom").val();
                }
                else {
                    last_subject_id = "0";
                    last_subject_name = "";
                    last_subject_credit = "";
                    last_subject_number = "";
                    last_subject_hour = "";
                    last_subject_timestart = "";
                    last_subject_timeend = "";
                    last_subject_date = "";
                    last_subject_classroom = "";
                }
                var s = subject_id + " " + subject_name + " ตอน " + subject_number;
                for (i = 0; i < 6; i++) {
                    for (j = 8; j <= 21; j++) {
                        tablecellcheck = $("#" + date2[i] + "id_" + j).find("p:contains("+ s +")").text().trim();
                        if (tablecellcheck == s) {
                            checkbool = true;
                        }
                    }
                }
                if (checkbool == false) {
                    $("#TableLocation tbody tr").find(".tablecheckbox").prop('checked', false);
                    $("#TableLocation tbody tr").css("background-color", "");

                    $(this).css("background-color", "#d6d9db");
                    $(this).find(".tablecheckbox").prop('checked', true);
                }
                else {
                    $("#TableLocation tbody tr").find(".tablecheckbox").prop('checked', false);
                    $("#TableLocation tbody tr").css("background-color", "");
                }
            }
        });
    //}
    //Table1();

    $("#TableLocation2").click(function () {
        if ($("#TableLocation tbody tr").find(".tablecheckbox").is(':checked')) {
            $("#TableLocation tbody tr").find(".tablecheckbox").prop('checked', false);
            $("#TableLocation tbody tr").css("background-color", "");
            var stringpart = "";
            var last_stringpart = "";
            var color = "";
            if (subject_credit.includes("-0-")) {
                color = "#25b0ee";
            }
            else {
                color = "#D3D3D3";
            }
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
                subject_timeend = parseInt(tmptimestart) + (parseInt(subject_timeend)  - parseInt(subject_timestart));
                stringpart = '<div id="x_button" class="btn x_button btn-default pull-right text-center">&times;</div><p>' + subject_id + ' ' + subject_name + ' ตอน ' + subject_number + '</p> '+'<input id="searchId" value="' + section_id + '" type="hidden">'
                    + '<input id="First_id_' + section_id + '" value="' + section_id + '" type="hidden">'
                    + '<input id="First_subjectid_' + section_id + '" value="' + subject_id + '" type="hidden">'
                    + '<input id="First_name_' + section_id + '" value="' + subject_name + '" type="hidden">'
                    + '<input id="First_number_' + section_id + '" value="' + subject_number + '" type="hidden">'
                    + '<input id="First_timestart_' + section_id + '" value="' + tmptimestart + '" type="hidden">'
                    + '<input id="First_timeend_' + section_id + '" value="' + subject_timeend + '" type="hidden">'
                    + '<input id="First_date_' + section_id + '" value="' + tmpdate + '" type="hidden">'
                    + '<input id="First_classroom_' + section_id + '" value="' + subject_classroom + '" type="hidden">'
                    + '<input id="Second_clasroom_' + section_id + '" value="' + subject_branch + '" type="hidden">';

                if (last_subject_id != "0") {
                    hour = parseInt(last_subject_timeend) - parseInt(last_subject_timestart);
                    last_subject_timeend = parseInt(subject_timeend) + parseInt(hour);
                    last_stringpart = '<input id="Second_id_' + section_id + '" value="' + last_section_id + '" type="hidden">'
                        + '<input id="Second_subjectid_' + section_id + '" value="' + last_subject_id + '" type="hidden">'
                        + '<input id="Second_name_' + section_id + '" value="' + last_subject_name + '" type="hidden">'
                        + '<input id="Second_number_' + section_id + '" value="' + last_subject_number + '" type="hidden">'
                        + '<input id="Second_timestart_' + section_id + '" value="' + subject_timeend + '" type="hidden">'
                        + '<input id="Second_timeend_' + section_id + '" value="' + last_subject_timeend + '" type="hidden">'
                        + '<input id="Second_date_' + section_id + '" value="' + tmpdate + '" type="hidden">'
                        + '<input id="Second_clasroom_' + section_id + '" value="' + last_subject_classroom + '" type="hidden">'
                    + '<input id="Second_clasroom_' + section_id + '" value="' + last_subject_branch + '" type="hidden">';
                    $("#" + tmp_id).html('<div class="" style="background-color:' + color + ';width:100%;height:50px">' + stringpart + last_stringpart + '</div>');
                }
                else {
                    $("#" + tmp_id).html('<div class="" style="background-color:' + color + ';width:100%;height:50px">' + stringpart + '</div>');
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
                replace();
                drag_drop();
            }
        }
    });
    $('#FIRST_SAVE_TIMESTART').on("change", function () {
        var TimeStart = $('#FIRST_SAVE_TIMESTART').val();
        var optionlisting = "";
        $('#FIRST_SAVE_TIMEEND option').remove();
        for (i = parseInt(TimeStart) + 1; i <= 21; i++) {
            if (i < 10) {
                optionlisting = optionlisting + '<option value="' + i + '">' + "0" + i + ":00" + '</option>';
            }
            else {
                optionlisting = optionlisting + '<option value="' + i + '">' + i + ":00" + '</option>';
            }

        }
        $('#FIRST_SAVE_TIMEEND').html(optionlisting);
    });
    $('#SECOND_SAVE_TIMESTART').on("change", function () {
        var TimeStart = $('#SECOND_SAVE_TIMESTART').val();
        var optionlisting = "";
        $('#SECOND_SAVE_TIMEEND option').remove();
        for (i = parseInt(TimeStart) + 1; i <= 21; i++) {
            if (i < 10) {
                optionlisting = optionlisting + '<option value="' + i + '">' + "0" + i + ":00" + '</option>';
            }
            else {
                optionlisting = optionlisting + '<option value="' + i + '">' + i + ":00" + '</option>';
            }

        }
        $('#SECOND_SAVE_TIMEEND').html(optionlisting);
    });

    function replace() {
        $("#TableLocation2 tr td div").on('click',function () {
            var optionlisting = "";
            var optionlisting2 = "";
            var TimeStart, TimeEnd, tmp_id2, tmp12, first_classroom,first_id, first_subjectid, first_name, first_number, first_branch, first_professor, first_timestart, first_timeend, first_date;
            var second_id, second_classroom, second_subjectid, second_name, second_number, second_branch, second_professor, second_timestart, second_timeend, second_date;
            $(".getdata").css("box-shadow", "0px 0px 0px");
            $(this).css("box-shadow", "4px 4px 6px");
            check_id = $(this).find("#searchId").val();
            first_id = $(this).find("#First_" + "id_" + check_id).val();
            first_subjectid = $(this).find("#First_" + "subjectid_" + check_id).val();
            first_name = $(this).find("#First_" + "name_" + check_id).val();
            first_number = $(this).find("#First_" + "number_" + check_id).val();
            first_timestart = $(this).find("#First_" + "timestart_" + check_id).val();
            first_timeend = $(this).find("#First_" + "timeend_" + check_id).val();
            first_date = $(this).find("#First_" + "date_" + check_id).val();
            first_classroom = $(this).find("#First_" + "classroom_" + check_id).val();
            first_branch = $(this).find("#First_" + "branch_" + check_id).val();

            $("#First_Header").html(first_subjectid + " " + first_name);
            $("#FIRST_SECTION_ID").val(first_id);
            $("#FIRST_SAVE_NUMBER").val(first_number);
            $("#FIRST_SAVE_DATE").val(first_date);
            $("#FIRST_SAVE_TIMESTART").val(first_timestart);
            $("#FIRST_SAVE_CLASSROOM").val(first_classroom);
            $("#FIRST_SAVE_BRANCH").val(first_branch);
            TimeStart = $('#FIRST_SAVE_TIMESTART').val();
            $('#FIRST_SAVE_TIMEEND option').remove();
            for (i = parseInt(TimeStart) + 1; i <= 21; i++) {
                if (i < 10) {
                    optionlisting = optionlisting + '<option value="' + i + '">' + "0" + i + ":00" + '</option>';
                }
                else {
                    optionlisting = optionlisting + '<option value="' + i + '">' + i + ":00" + '</option>';
                }

            }
            $('#FIRST_SAVE_TIMEEND').html(optionlisting);
            $("#FIRST_SAVE_TIMEEND").val(first_timeend);

            second_id = $(this).find("#Second_" + "id_" + check_id).val();
            if (second_id != null && second_id != "0") {
                second_id = $(this).find("#Second_" + "id_" + check_id).val();
                second_subjectid = $(this).find("#Second_" + "subjectid_" + check_id).val();
                second_name = $(this).find("#Second_" + "name_" + check_id).val();
                second_number = $(this).find("#Second_" + "number_" + check_id).val();
                second_timestart = $(this).find("#Second_" + "timestart_" + check_id).val();
                second_timeend = $(this).find("#Second_" + "timeend_" + check_id).val();
                second_date = $(this).find("#Second_" + "date_" + check_id).val();
                second_classroom = $(this).find("#Second_" + "classroom_" + check_id).val();
                second_branch = $(this).find("#Second_" + "branch_" + check_id).val();

                $("#Second_Header").html(second_subjectid + " " + second_name);
                $("#SECOND_SECTION_ID").val(second_id);
                $("#SECOND_SAVE_NUMBER").val(second_number);
                $("#SECOND_SAVE_DATE").val(second_date);
                $("#SECOND_SAVE_TIMESTART").val(second_timestart);
                $("#SECOND_SAVE_CLASSROOM").val(second_classroom);
                $("#SECOND_SAVE_BRANCH").val(second_branch);
                TimeEnd = $('#SECOND_SAVE_TIMESTART').val();
                $("#gate").val(TimeEnd);
                $('#SECOND_SAVE_TIMEEND option').remove();
                for (i = parseInt(TimeEnd) + 1; i <= 21; i++) {
                    if (i < 10) {
                        optionlisting2 = optionlisting2 + '<option value="' + i + '">' + "0" + i + ":00" + '</option>';
                    }
                    else {
                        optionlisting2 = optionlisting2 + '<option value="' + i + '">' + i + ":00" + '</option>';
                    }

                }
                $('#SECOND_SAVE_TIMEEND').html(optionlisting2);
                $("#SECOND_SAVE_TIMEEND").val(second_timeend);
            }
            else{
                $("#Second_Header").html("ปฎิบัติ");
                $("#SECOND_SECTION_ID").val("0");
                $("#SECOND_SAVE_NUMBER").val("");
                $("#SECOND_SAVE_DATE").val("M");
                $("#SECOND_SAVE_CLASSROOM").val("");
                $("#SECOND_SAVE_TIMESTART").val(8);
                $("#SECOND_SAVE_BRANCH").val("");
                TimeEnd = $('#SECOND_SAVE_TIMESTART').val();
                $('#SECOND_SAVE_TIMEEND option').remove();
                for (i = parseInt(TimeEnd) + 1; i <= 21; i++) {
                    if (i < 10) {
                        optionlisting2 = optionlisting2 + '<option value="' + i + '">' + "0" + i + ":00" + '</option>';
                    }
                    else {
                        optionlisting2 = optionlisting2 + '<option value="' + i + '">' + i + ":00" + '</option>';
                    }

                }
                $('#SECOND_SAVE_TIMEEND').html(optionlisting2);
                $("#SECOND_SAVE_TIMEEND").val(9);
            }
        });
    }
    replace();
    $("#TableLocation2 tr td").hover(function () {
        tmp_id = $(this).attr("id");
        if (tmp_id != null) {
            tmp1 = tmp_id.split("_");
            tmp2 = parseInt(tmp1[1]) - 1;
            tmp3 = tmp1[0] + "_" + tmp2;
            tmp4 = parseInt(tmp1[1]) + 1;
            tmp5 = tmp1[0] + "_" + tmp4;

            tmp7 = tmp_id.split("id_");
            tmpdate = tmp7[0];
            tmptimestart = parseInt(tmp7[1]);

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

            tmp7 = tmp_id.split("id_");
            tmpdate = tmp7[0];
            tmptimestart = parseInt(tmp7[1]);

            colspanvalue = (parseInt(tmp1[1]) - 1) + parseInt(subject_hour);
            for (value = parseInt(tmp1[1]); value <= colspanvalue; value++) {
                var aa = tmp1[0] + "_" + value;
                $("#" + aa).css("background-color", "");
            }
        }
    });

});
////////////////////////////////////////////////
