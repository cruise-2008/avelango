function profileInit() {

    var months = [];
    var counts = [];

    // left button menu
    $("#profile_btn_mypage").bind('click', function () { buttonhighlight($(this)); showmypage(); });
    $("#profile_btn_result").bind('click', function () { buttonhighlight($(this)); showresult(); });
    $("#profile_btn_lastop").bind('click', function () { buttonhighlight($(this)); showlastop(); });

    var userdata = {};
    var findedusers = {};
    var openeduser = {};
    var datatable;

    function showmypage() {
        cleanDataTable();
        drawtable(userdata, true);
    }

    function showresult() {
        cleanDataTable();
        findedusersdisplay(findedusers);
        $('.dt-toolbar-footer').show();
        $('.dt-toolbar').show();
    }

    function showlastop() {
        cleanDataTable();
        drawtable(openeduser, false);
    }

    function cleanDataTable() {
        var body = $('#profile_tbd_userdata').empty();
        body.parent().find('thead').empty();
        $('.dt-toolbar-footer').hide();
        $('.dt-toolbar').hide();
    }

    function buttonhighlight(thi5) {
        $("#profile_btn_mypage").removeClass('active');
        $("#profile_btn_result").removeClass('active');
        $("#profile_btn_lastop").removeClass('active');
        thi5.addClass('active');
    }

    $(function () {
        $(":file").bind('change', function () {
            var img = $('#profile_img');

            $('.jcrop-holder').remove();
            var jcrop = img.data('Jcrop');
            if (jcrop != null) {
                jcrop.destroy();
            }
            $('#profile_img').show().css('visibility', 'visible').width('');
            if (this.files && this.files[0]) {
                var reader = new window.FileReader();
                reader.onload = imageIsLoaded;
                reader.readAsDataURL(this.files[0]);
                $('#profile_logoimg').modal('show');
            }
        });
    });

    function imageIsLoaded(e) {
        var target = e.target.result;
        var jcropApi;
        $('#profile_img').attr('src', target);
        var img = $('#profile_img');
        var h = img.height();
        var w = img.width();

        img.Jcrop({
            setSelect: [1, 1, w, h],
            onChange: function (coords) {
                $('#profile_x0').attr('value', coords.x);
                $('#profile_y0').attr('value', coords.y);
            },
            onRelease: releaseCheck
        }, function () {
            jcropApi = this;
            jcropApi.animateTo([100, 100, 300, 300]);
            $('#can_click,#can_move,#can_size').attr('checked', 'checked');
            $('#ar_lock,#size_lock,#bg_swap').attr('checked', false);
            $('.requiresjcrop').show();
        });

        jcropApi.setOptions({ aspectRatio: 3 / 3 });
        jcropApi.focus();

        function releaseCheck() {
            jcropApi.setOptions({
                allowSelect: true
            });
            $('#can_click').attr('checked', false);
        };


        var offset = (580 - ($('#profile_img').width())) / 2;
        $('.modal-content').attr('style', 'margin-left: ' + offset + 'px;');
        $('.modal-content').width(($('#profile_img').width()) + 40);
        $('.jcrop-keymgr').attr('type', 'hidden');
    };


    $("#profile_set_img_to_logo").bind('click', function () {
        $('#profile_h0').attr('value', $('#profile_img').height());
        
        var sqrs = $($('.jcrop-holder .jcrop-tracker'));
        var a = sqrs[0];
        var b = sqrs[1];

        $('#profile_x0').val($(a).offset().left - $(b).offset().left);
        $('#profile_y0').val($(a).offset().top - $(b).offset().top);
        $('#profile_h0').val($(a).height());
        $('#profile_h1').val($(b).height());
        $('#profile_w1').val($(b).width());

        var formData = new window.FormData($('#inputform')[0]);
        $.ajax({
            xhr: function () {
                return $.ajaxSettings.xhr();
            },
            type: "POST",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            url: "/Profile/AddUserLogo",
            complete: function () {
                $.ajax({
                    type: "POST",
                    traditional: true,
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    url: "/Profile/GetUserImage",
                    success: function (data) {
                        if (data) {
                            $('#profile_img_ava').attr('src', data);
                            $.smallBox({ title: texts.Pers_logo_successfully, color: "#339999", iconSmall: "fa fa-thumbs-up bounce animated", timeout: 1024, });
                        } else {
                            $.smallBox({ title: texts.Pers_logo_unsuccessfully, color: "#880000", iconSmall: "fa fa-thumbs-up bounce animated", timeout: 2048, });
                        }
                    },
                    error: function () {
                        $.smallBox({ title: texts.Pers_logo_unsuccessfully, color: "#880000", iconSmall: "fa fa-thumbs-up bounce animated", timeout: 2048, });
                    }
                });
            }
        });
    });

    $("#profile_remove_ava").click(function () {
        document.cookie = "cancelTitleChange=true";
        $.ajax({
            type: "POST",
            traditional: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            url: "/Profile/DeleteUserLogo",
            success: function (result) {
                if (result.Deleted) {
                    $("#deleteLogo").hide();
                    $.smallBox({ title: texts.Pers_logo_del_successfully, color: "#339999", iconSmall: "fa fa-thumbs-up bounce animated", timeout: 1024, });
                    $("#userLogoArticle img").attr('src', result.LogoPath);
                } else {
                    $.smallBox({ title: texts.Pers_logo_del_unsuccessfully, color: "#880000", iconSmall: "fa fa-thumbs-up bounce animated", timeout: 1024, });
                }

            },
            error: function () {
                $.smallBox({ title: texts.Pers_logo_del_unsuccessfully, color: "#880000", iconSmall: "fa fa-thumbs-up bounce animated", timeout: 1024, });
            },
        });
    });

    $("#profile_add_new_ava").bind('click', function () {
        $("#profile_file").click();
    });

    $(function () {
        $('#profile_btns_ava').fadeTo('fast', 0.1);
        $.ajax({
            type: "POST",
            traditional: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            url: "/Profile/GetUserData",
            success: function (data) {
                userdata = data;
                if (userdata) {
                    $('#profile_spn_creation_data').text(' ' + userdata.AccountCreated);
                    if (!userdata.Gender) {
                        $('#carusel_img_1').attr('src', "/Content/img/profile/s1.png"); $('#carusel_img_2').attr('src', "/Content/img/profile/s2.png");
                        $('#carusel_img_3').attr('src', "/Content/img/profile/s3.png"); $('#carusel_img_4').attr('src', "/Content/img/profile/s4.png");
                    }
                    if (userdata.Gender == "male") {
                        $('#carusel_img_1').attr('src', "/Content/img/profile/m1.png"); $('#carusel_img_2').attr('src', "/Content/img/profile/m2.png");
                        $('#carusel_img_3').attr('src', "/Content/img/profile/m3.png"); $('#carusel_img_4').attr('src', "/Content/img/profile/m4.png");
                    }
                    if (userdata.Gender == "female") {
                        $('#carusel_img_1').attr('src', "/Content/img/profile/f1.png"); $('#carusel_img_2').attr('src', "/Content/img/profile/f2.png");
                        $('#carusel_img_3').attr('src', "/Content/img/profile/f3.png"); $('#carusel_img_4').attr('src', "/Content/img/profile/f4.png");
                    }
                    $('#profile_carusele_images').show();

                    drawtable(userdata, true);
                    startCarusel();
                    getNSetAvaImage('');
                }
            },
            error: function () { }
        });
    });

    function getNSetAvaImage(mail) {
        $.ajax({
            type: "POST",
            traditional: true,
            data: JSON.stringify({ 'mail': mail }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            url: "/Profile/GetUserImage",
            success: function (imgpath) {
                if (imgpath) {
                    $('#profile_img_ava').attr('src', imgpath);
                }
                else {
                    $('#profile_img_ava').attr('src', '/Content/img/avatars/default.png');
                }
            },
            error: function () { $('#profile_img_ava').attr('src', '/Content/img/avatars/default.png'); }
        });
    }


    // variable for I18N
    var textsave, textcancel, male, female, newpassword, confirmpassword;

    function edittable() {
        $('.fa-edit.fa-2x').bind('click', function () {
            if ($('.btn_save').length != 0) { return; }
            var field = $(this).parent().prev();
            field.find('span').remove();
            var span = field.append($(document.createElement("span")));
            var buttons = $(document.createElement("div")).attr('style', 'width:20%')
                .append($(document.createElement("a")).addClass("btn bg-color-teal txt-color-white btn-xs btn_cancel").attr('href', '#').text(textcancel)).
                append($(document.createElement("a")).addClass("btn btn-warning btn-xs btn_save").attr('href', '#').text(textsave));
            switch (field.find('small').attr('id')) {

                case 'username': {
                    span.append($(document.createElement("input")).attr('type', 'text').attr('style', 'border: 0; width: 90%').attr('autofocus', true).attr('value', userdata.FullName).attr('id', 'int_username')).
                        append(buttons);
                    break;
                }

                case 'birthday': {
                    //day
                    span.parent().find('span').empty();
                    var subspan = $(document.createElement("form")).addClass('smart-form');
                    var selectDay = $(document.createElement("select")).attr('name', 'day').attr('style', 'margin-top: 0px; border: 0');
                    selectDay.append($(document.createElement("option")).attr('value', 0).attr('disabled', true).text('Day'));
                    for (var i = 1; i < 32; i++) {
                        var active = false;
                        var day = i.toString();
                        if (parseInt(day) == parseInt(userdata.BirthdayDay)) { active = true; } else { active = false; }
                        if (day.length < 2) { day = '0' + day; }
                        selectDay.append($(document.createElement("option")).attr('selected', active).attr('value', i).text(day));
                    }
                    subspan.append($(document.createElement("section")).addClass('col col-3 birthdays').attr('style', 'padding-left: 0px;margin-bottom: 0px;').
                        append($(document.createElement("label")).addClass('select').
                            append(selectDay)));
                    //month
                    var selectMonth = $(document.createElement("select")).attr('name', 'month').attr('style', 'margin-top: 0px; border: 0');
                    selectMonth.append($(document.createElement("option")).attr('value', 0).attr('disabled', true).text('Month'));
                    $.each(months, function (index, value) {
                        active = false;
                        if (index + 1 == userdata.BirthdayMonth) { active = true; } else { active = false; }
                        selectMonth.append($(document.createElement("option")).attr('selected', active).attr('value', index + 1).text(value));
                    });
                    subspan.append($(document.createElement("section")).addClass('col col-3 birthdays').attr('style', 'padding-left: 0px;margin-bottom: 0px;').
                        append($(document.createElement("label")).addClass('select').
                            append(selectMonth)));
                    //year
                    var selectYear = $(document.createElement("select")).attr('name', 'year').attr('style', 'margin-top: 0px; border: 0');
                    selectYear.append($(document.createElement("option")).attr('value', 0).attr('disabled', true).text('Year'));
                    for (var j = 1950; j < 2000; j++) {
                        active = false;
                        if (j == userdata.BirthdayYear) { active = true; } else { active = false; }
                        selectYear.append($(document.createElement("option")).attr('selected', active).attr('value', j - 1949).text(j));
                    }
                    subspan.append($(document.createElement("section")).addClass('col col-3 birthdays').attr('style', 'padding-left: 0px;margin-bottom: 0px;').
                       append($(document.createElement("label")).addClass('select').
                         append(selectYear))).append(buttons);
                    subspan.appendTo(span);
                    break;
                }

                case 'gender': {
                    var activemale = true; var activefemale = false;
                    if (userdata.Gender == 'female') { activemale = false; activefemale = true; }
                    span.append($(document.createElement("form")).addClass('smart-form').
                        append($(document.createElement("label")).addClass("radio").
                            append($(document.createElement("input")).addClass('radiobox').attr('name', 'radio-inline').attr('type', 'radio').attr('value', 'male').attr('checked', activemale)).
                            append($(document.createElement("i"))).prepend(male)).
                        append($(document.createElement("label")).addClass("radio").
                            append($(document.createElement("input")).addClass('radiobox').attr('name', 'radio-inline').attr('value', 'female').attr('type', 'radio').attr('checked', activefemale)).
                            append($(document.createElement("i"))).prepend(female)).
                        append(buttons)
                    );
                    break;
                }

                case 'telephone': {
                    span.append($(document.createElement("input")).attr('type', 'tel').attr('name', 'phone').attr('placeholder', 'Phone').attr('data-mask', '(999) 999-9999').attr('value', userdata.MobilePhone).attr('autofocus', true).attr('style', 'border: 0; width: 90%').attr('id', 'int_userphone')).
                        append(buttons);
                    masked();
                    break;
                }

                case 'email': {
                    span.append($(document.createElement("input")).attr('type', 'text').attr('style', 'border: 0; width: 90%').attr('autofocus', true).attr('value', userdata.Email).attr('id', 'int_usermail')).
                        append(buttons);
                    break;
                }

                case 'skype': {
                    span.append($(document.createElement("input")).attr('type', 'text').attr('style', 'border: 0; width: 90%').attr('autofocus', true).attr('value', userdata.Skype).attr('id', 'int_userskype')).
                        append(buttons);
                    break;
                }

                case 'location': {
                    subspan = $(document.createElement("form")).addClass('smart-form');

                    var selectCountry = $(document.createElement("select")).attr('name', 'month').attr('style', 'margin-top: 10px; border: 0');
                    selectCountry.append($(document.createElement("option")).attr('value', 0).attr('disabled', true).text('Month'));
                    $.each(counts, function (index, value) {
                        active = false;
                        if (counts[index] == userdata.Location) { active = true; } else { active = false; }
                        selectCountry.append($(document.createElement("option")).attr('selected', active).attr('value', index + 1).text(value));
                    });
                    subspan.append($(document.createElement("section")).addClass('col col-3 birthdays').attr('style', 'padding-left: 0px;').
                        append($(document.createElement("label")).addClass('select').
                            append(selectCountry)));
                    subspan.appendTo(span).
                        append(buttons);
                    break;
                }

                case 'loginpassword': {
                    span.append($(document.createElement("input")).attr('type', 'text').attr('style', 'border: 0; width: 35%; padding-left: 5px;').attr('value', newpassword).attr('id', 'inp_userpass').attr('autofocus', true)).
                        append($(document.createElement("input")).attr('type', 'text').attr('style', 'border: 0; width: 35%; margin-left: 10px; padding-left: 5px;').attr('value', confirmpassword).attr('id', 'inp_confirm_userpass')).
                              append(buttons);
                    $('#inp_userpass').select();

                    $('#inp_userpass').bind('keypress', function () {
                        var userpass = $('#inp_userpass');
                        if (userpass.attr('type') != 'password') { $('#inp_userpass').attr('type', 'password').val(''); $('#inp_confirm_userpass').attr('type', 'password').val(''); }
                    });

                    $('#inp_confirm_userpass').bind('keypress', function () {
                        var userpass = $('#inp_userpass');
                        if (userpass.attr('type') != 'password') { $('#inp_userpass').attr('type', 'password').val(''); $('#inp_confirm_userpass').attr('type', 'password').val(''); }
                    });
                    break;
                }

                case 'bugtrackerlogin': {
                    span.append($(document.createElement("input")).attr('type', 'text').attr('style', 'border: 0; width: 90%').attr('autofocus', true).attr('value', userdata.BugTrackerLogin).attr('id', 'inp_btlogin')).
                        append(buttons);
                    break;
                }

                case 'bugtrackerpassword': {
                    span.append($(document.createElement("input")).attr('type', 'text').attr('style', 'border: 0; width: 35%; padding-left: 5px;').attr('value', newpassword).attr('id', 'inp_bt_userpass').attr('autofocus', true)).
                       append($(document.createElement("input")).attr('type', 'text').attr('style', 'border: 0; width: 35%; margin-left: 10px; padding-left: 5px;').attr('value', confirmpassword).attr('id', 'inp_bt_confirm_userpass')).
                       append(buttons);
                    $('#inp_bt_userpass').select();

                    $('#inp_bt_userpass').bind('keypress', function () {
                        var btuserpass = $('#inp_bt_userpass');
                        if (btuserpass.attr('type') != 'password') { btuserpass.attr('type', 'password').val(''); $('#inp_bt_confirm_userpass').attr('type', 'password').val(''); }
                    });

                    $('#inp_bt_confirm_userpass').bind('keypress', function () {
                        var btuserpass = $('#inp_bt_userpass');
                        if (btuserpass.attr('type') != 'password') { btuserpass.attr('type', 'password').val(''); $('#inp_bt_confirm_userpass').attr('type', 'password').val(''); }
                    });
                    break;
                }
            }
            $('.btn_cancel').bind('click', function () { showmypage(); });

            $('.btn_save').bind('click', function () {
                var el = $(this).parent().parent().parent();
                switch (el.find('small').attr('id')) {

                    case 'username': {
                        var username = $('#int_username').val();
                        if (/^[a-zA-Z0-9- ]*$/.test(username) == false) {
                            $.smallBox({ title: "Warning, name contains illegal characters!", color: "#CD5C5C", iconSmall: "fa fa-user", timeout: 2024 });
                            break;
                        }
                        var jdata0 = JSON.stringify({ 'username': username });
                        sendChanges("Profile", "SetUserName", jdata0, function () { userdata.FullName = username; });
                        break;
                    }

                    case 'birthday': {
                        var bd = $('.birthdays');
                        var d = bd.first().find(":selected").text();
                        var m = bd.first().next().find(":selected").text();
                        var y = bd.last().find(":selected").text();
                        var jdata1 = JSON.stringify({ 'day': d, 'month': m, 'year': y });
                        sendChanges("Profile", "SetBirthday", jdata1, function () { userdata.BirthdayDay = d; userdata.BirthdayMonth = m; userdata.BirthdayYear = y; });
                        break;
                    }

                    case 'gender': {
                        var gnr = 'male';
                        if (el.find('input').last().is(':checked')) { gnr = 'female'; }
                        var jdata2 = JSON.stringify({ 'gender': gnr });
                        sendChanges("Profile", "SetGender", jdata2, function () { userdata.Gender = gnr; });
                        break;
                    }

                    case 'telephone': {
                        var telephone = ($('#int_userphone').val().match(/\d+/g)).join('');
                        var jdata3 = JSON.stringify({ 'mobilePhone': telephone });
                        sendChanges("Profile", "SetMobilePhone", jdata3, function () { userdata.MobilePhone = telephone; });
                        break;
                    }

                    case 'email': {
                        var ml = $('#int_usermail').val();
                        var regex = /^([a-zA-Z0-9_.+-])+\@@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                        if (!regex.test(ml)) {
                            $.smallBox({ title: "Warning, email contains illegal characters!", color: "#CD5C5C", iconSmall: "fa fa-user", timeout: 2024 });
                            break;
                        }
                        var jdata4 = JSON.stringify({ 'email': ml });
                        sendChanges("Profile", "SetEmail", jdata4, function () { userdata.Email = ml; });
                        break;
                    }

                    case 'skype': {
                        var skp = $('#int_userskype').val();
                        if (/^[a-zA-Z0-9- ]*$/.test(skp) == false) {
                            $.smallBox({ title: "Warning, skype-name contains illegal characters!", color: "#CD5C5C", iconSmall: "fa fa-user", timeout: 2024 });
                            break;
                        }
                        var jdata5 = JSON.stringify({ 'skype': skp });
                        sendChanges("Profile", "SetSkype", jdata5, function () { userdata.Skype = skp; });
                        break;
                    }

                    case 'location': {
                        var lctn = $('#location').parent().find('select :checked').text();
                        var jdata6 = JSON.stringify({ 'location': lctn });
                        sendChanges("Profile", "SetLocation", jdata6, function () { userdata.Location = lctn; });
                        break;
                    }

                    case 'loginpassword': {
                        var loginpassword = $('#inp_userpass').val();
                        var confirmLoginpassword = $('#inp_confirm_userpass').val();
                        if ((loginpassword === confirmLoginpassword) && (loginpassword.length > 5)) {
                            var jdata7 = JSON.stringify({ 'loginpassword': loginpassword });
                            sendChanges("Profile", "SetLoginPassword", jdata7, function () { });
                        }
                        else {
                            $.smallBox({ title: "Password isn't equivalent, confirm-password or less than 6 characters!", color: "#CD5C5C", iconSmall: "fa fa-user", timeout: 2024 });
                            break;
                        }
                        break;
                    }

                    case 'bugtrackerlogin': {
                        var bugtrackerlogin = $('#inp_btlogin').val();
                        var jdata8 = JSON.stringify({ 'bugTrackerLogin': bugtrackerlogin });
                        sendChanges("Profile", "SetBugTrackerLogin", jdata8, function () { userdata.BugTrackerLogin = bugtrackerlogin; });
                        break;
                    }

                    case 'bugtrackerpassword': {
                        var bugtrackerpassword = $('#inp_bt_userpass').val();
                        var confirmbugtrackerpassword = $('#inp_bt_confirm_userpass').val();
                        if ((bugtrackerpassword === confirmbugtrackerpassword) && (bugtrackerpassword.length > 5)) {
                            var jdata9 = JSON.stringify({ 'bugTrackerPassword': bugtrackerpassword });
                            sendChanges("Profile", "SetBugTrackerPassword", jdata9, function () { });
                        }
                        else {
                            $.smallBox({ title: "Password isn't equivalent, confirm-password or less than 6 characters!", color: "#CD5C5C", iconSmall: "fa fa-user", timeout: 2024 });
                            break;
                        }
                        break;
                    }

                }
            });
        });
    }

    function sendChanges(cont, meth, jdata, actionCallBack) {
        $.ajax({
            type: "POST",
            traditional: true,
            dataType: 'json',
            data: jdata,
            contentType: "application/json; charset=utf-8",
            url: '/' + cont + '/' + meth,
            success: function (datum) {
                if (datum) {
                    actionCallBack();
                    cleanDataTable();
                    drawtable(userdata, true);
                    $.smallBox({ title: "Changed successfully!", color: "#739E73", iconSmall: "fa fa-user", timeout: 1024 });
                }
                else {
                    cleanDataTable();
                    drawtable(userdata, true);
                    $.smallBox({ title: "Changed unsuccessfully!", color: "#CD5C5C", iconSmall: "fa fa-user", timeout: 1024 });
                }
            },
            error: function () {
                cleanDataTable();
                drawtable(userdata, true);
                $.smallBox({ title: "Changed unsuccessfully!", color: "#CD5C5C", iconSmall: "fa fa-user", timeout: 1024 });
            }
        });
    }

    var birthday, gender, phone, email, skype, location, loginpass, btLogin, btPass;

    function drawtable(udata, iam) {
        var editable = iam == true ? 'fa fa-edit fa-2x margin-top-10' : '';
        var body = $('#profile_tbd_userdata');
        //fullname
        body.append($(document.createElement("tr")).addClass('tr_line')
            .append($(document.createElement("td")))
            .append($(document.createElement("td"))
                .append($(document.createElement("h1"))
                    .append($(document.createElement("small")).addClass('row').attr('id', 'username').attr('style', 'height:0; margin:0'))
                    .append($(document.createElement("span")).addClass('semi-bold').text(udata.FullName)
                    )))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //birthday
        var monthz = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        var monthnumber = '0';
        for (var r = 0; r < 12; r++) { if (monthz[r] == udata.BirthdayMonth || r == udata.BirthdayMonth) { monthnumber = r; break; } }
        body.append($(document.createElement("tr")).addClass('tr_odd_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-gift fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'birthday').text(birthday))
                .append($(document.createElement("span")).addClass('txt-color-darken changemonth margin-right-5').text(udata.BirthdayDay))
                .append($(document.createElement("span")).addClass('txt-color-darken changemonth margin-right-5').attr('id', 'id_month').attr('accesskey', monthnumber).text(udata.BirthdayMonth))
                .append($(document.createElement("span")).addClass('txt-color-darken changemonth margin-right-5').text(udata.BirthdayYear)))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //gender
        var gendero = udata.Gender ? udata.Gender : '-';
        body.append($(document.createElement("tr")).addClass('tr_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-male fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'gender').text(gender))
                .append($(document.createElement("span")).addClass('txt-color-darken').text(gendero)
                ))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //telephone
        var number = udata.MobilePhone;
        if (number) { number = number.replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3"); }
        var telephone = number ? number : '-';
        body.append($(document.createElement("tr")).addClass('tr_odd_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-phone fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'telephone').text(phone))
                .append($(document.createElement("span")).addClass('txt-color-darken').text(telephone)
                ))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //email                    
        body.append($(document.createElement("tr")).addClass('tr_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-envelope fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'email').text(email))
                .append($(document.createElement("span")).addClass('txt-color-darken').text(udata.Email)
                ))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //skype
        var skypo = udata.Skype ? udata.Skype : '-';
        body.append($(document.createElement("tr")).addClass('tr_odd_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-skype fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'skype').text(skype))
                .append($(document.createElement("span")).addClass('txt-color-darken').text(skypo)
                ))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //location
        body.append($(document.createElement("tr")).addClass('tr_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-map-marker fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'location').text(location))
                .append($(document.createElement("span")).addClass('txt-color-darken').text(udata.Location)
                ))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //security line
        //body.append($(document.createElement("tr"))).append($(document.createElement("div")).addClass('font-lg security').text('Security part'));
        if (!iam) { edittable(); return; }

        //password        
        body.append($(document.createElement("tr")).addClass('tr_odd_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-lock fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'loginpassword').text(loginpass))
                .append($(document.createElement("span")).addClass('txt-color-darken').text("*********")
                ))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //bugTrackerLogin                                                                                                        
        var bugTrackerLogin = udata.BugTrackerLogin ? udata.BugTrackerLogin : '-';
        body.append($(document.createElement("tr")).addClass('tr_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-user fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'bugtrackerlogin').text(btLogin))
                .append($(document.createElement("span")).addClass('txt-color-darken').text(bugTrackerLogin)
                ))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        //bugTrackerpassword
        body.append($(document.createElement("tr")).addClass('tr_odd_line')
            .append($(document.createElement("td")).append($(document.createElement("i")).addClass('fa fa-lock fa-2x margin-left-10 margin-top-5')))
            .append($(document.createElement("td"))
                .append($(document.createElement("small")).addClass('row').attr('style', 'margin:0').attr('id', 'bugtrackerpassword').text(btPass))
                .append($(document.createElement("span")).addClass('txt-color-darken').text("*********")
                ))
            .append($(document.createElement("td")).append($(document.createElement("span")).addClass(editable)))
        );
        edittable();
    }

    $('#profile_btns_ava').bind('mouseover', function () { $('#profile_btns_ava').fadeTo('fast', 0.8); });
    $('#profile_btns_ava').bind('mouseleave', function () { $('#profile_btns_ava').fadeTo('fast', 0.0); });

    //Masked input
    function masked() {
        $('input[data-mask]').each(function () {
            $(this).mask($(this).data('mask'));
        });
    }

    // Slider carusel
    function startCarusel() {
         var j = new $JssorSlider$("profile_sl_container", {
             $AutoPlay: true, $AutoPlayInterval: 10000, $SlideDuration: 10000
         });
    };

    caruseleresize();
    function caruseleresize() {
        var outsdivwidth = $('.col-sm-12 div.row').first().width();
        $('#profile_sl_container').css('width', outsdivwidth);
        $('#profile_sl_container div').first().css('width', outsdivwidth);
    }
    $(window).resize(function () { caruseleresize(); });



    //USERS SEARCH
    $("#profile_btn_search").click(function () { search(); });

    $(document).keypress(function (e) {
        if ((e.which == 13) && ($("#profile_searchTextBox").is(":focus"))) {
            search();
        }
    });

    function search() {
        $('#profile_btn_result').attr('disabled', false);
        $('#profile_btn_mypage').attr('disabled', false);
        $("#profile_loader").show();
        var temp = {
            template: $("#profile_searchTextBox").val(),
            needToRoles: true
        };
        var jTemp = JSON.stringify(temp);
        $.ajax({
            type: "POST",
            url: "/Profile/GetUsersByCurrentProject",
            traditional: true,
            data: jTemp,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                findedusers = data;
                showresult();
            },
            error: function () {
                $.SmartMessageBox({ title: texts.Search_user_completed_with_error, content: texts.Please_try_again_later, });
                $(".MessageBoxContainer").css('background-color', '#CD5C5C');
            },
            complete: function () {
                $("#profile_loader").hide();
            },
        });
    }

    function findedusersdisplay(data) {
        if (datatable) { datatable.fnClearTable(); }
        $('#profile_tbd_userdata').parent().find('thead').append($(document.createElement("tr"))
            .append($(document.createElement("th")).attr('style', 'padding-left: 30px;').text('User name'))
            .append($(document.createElement("th")).attr('style', 'padding-left: 20px').text('Email'))
            .append($(document.createElement("th")).attr('style', 'padding-left:  0px').text('Role'))
            .append($(document.createElement("th")).attr('style', 'padding-left:  0px').text(''))
        );
        if (!datatable) { userSearchDataTableInit(); }
        for (var i = 0; i < data.FullNames.length; i++) {
            datatable._fnAddTr('<tr style="max-height: 100px; height: 44px">' +
                                   '<td id="userTh_" style="padding-left: 30px">' + data.FullNames[i] + '</td>' +
                                   '<td id="mailTh_" style="padding-left: 20px">' + data.Mails[i] + '</td>' +
                                   '<td id="roleTh_" style="padding-left: 0px">' + data.Roles[i] + '</td>' +
                                   '<td style="padding-left: 0px"><a class="viewacc" href="#"></a ></td>' +
                               '</tr>');
        }
        datatable.fnDraw();

        //  view user details handler
        var $nodes = $(datatable.fnGetNodes());
        $nodes.find('.viewacc').parent().parent().bind('click', function () {
            $('#profile_btn_lastop').attr('disabled', false);
            var mail = { mail: $(this).find('td[id="mailTh_"]').text(), };
            var jMail = JSON.stringify(mail);
            $.ajax({
                type: "POST",
                url: "/Profile/GetUserDetailsByMail",
                traditional: true,
                data: jMail,
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (userDetail) {
                    if (userDetail) {
                        openeduser = userDetail;
                        showlastop();
                        var imagepath = getNSetAvaImage(userDetail.Email);
                        $('#profile_img_ava').attr('src', imagepath);
                    }
                }
            });
        });
    }

    function userSearchDataTableInit() {
        var responsiveHelperDtBasic = undefined;
        var breakpointDefinition = { tablet: 1024, phone: 480 };
        datatable = $('#profile_dt_basic').dataTable({
            "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-6'f><'col-sm-6 col-xs-12 hidden-xs'l>r>" + "t" + "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
            "autoWidth": true,
            "preDrawCallback": function () { if (!responsiveHelperDtBasic) { responsiveHelperDtBasic = new ResponsiveDatatablesHelper($('#profile_dt_basic'), breakpointDefinition); } },
            "rowCallback": function (nRow) { responsiveHelperDtBasic.createExpandIcon(nRow); },
            "drawCallback": function (oSettings) { responsiveHelperDtBasic.respond(); }
        });
        $('.dt-toolbar').attr('style', 'background: #585858');
    }

    if ('@ViewBag.Highlight' == 'bt') {
        setTimeout(function () {
            $('.fa-user.fa-2x').css('color', '#f00000').animate({ 'color': '#484848' }, 4095);
            $('.fa-lock.fa-2x').last().css('color', '#f00000').animate({ 'color': '#484848' }, 4095);
        }, 1500);
    }

    //----------------------------------------------------------------------------

    $(function () {
        var lang = $.getlang();
        $.changePrimaryLayoutLang(lang);
        setprofilelang(lang);
        $.setflag('', lang);

        $('#lang_set ul li').bind('click', function () {
            var newlang = $(this).text().trim().toLowerCase().match(/\((.*)\)/)[1];
            var result = $.setlang(newlang.trim());
            if (result) {
                $.changePrimaryLayoutLang(newlang);
                setprofilelang(newlang);
                $.setflag($(this), '');
            }
        });

        var texts = {};
        function setprofilelang(tnewlang) {
            $.ajax({
                type: "POST",
                data: { lang: tnewlang },
                url: "/I18N/GetTextOnAnLanguage_Profile",
                success: function (data) {
                    if (data) {
                        var arrObj = $.parseJSON(data);
                        for (var i = 0; i < arrObj.length ; i++) {
                            texts[arrObj[i].Key] = arrObj[i].Value;
                        }

                        birthday = texts.Birthday; $('#birthday').text(texts.Birthday);

                        gender = texts.Gender; $('#gender').text(texts.Gender);

                        phone = texts.Mobile_Phone; $('#telephone').text(texts.Mobile_Phone);

                        email = texts.Email; $('#email').text(texts.Email);

                        skype = texts.Skype; $('#skype').text(texts.Skype);

                        location = texts.Location; $('#location').text(texts.Location);

                        loginpass = texts.Login_password; $('#loginpassword').text(texts.Login_password);

                        btLogin = texts.Bug_tracker_login; $('#bugtrackerlogin').text(texts.Bug_tracker_login);

                        btPass = texts.Bug_tracker_password; $('#bugtrackerpassword').text(texts.Bug_tracker_password);

                        $('#profile_btn_mypage span').text(texts.My_page);
                        $('#profile_btn_result span').text(texts.Result_search);
                        $('#profile_btn_lastop span').text(texts.Last_opened);

                        $('#profile_trans_creation_date').text(texts.Creation_date);
                        $('#profile_searchTextBox').attr('placeholder', texts.Search_users + '..');

                        // non drawed
                        counts = [texts.ct_Afghanistan, texts.ct_Albania, texts.ct_Algeria, texts.ct_Andorra, texts.ct_Angola, texts.ct_Antarctica, texts.ct_Antigua_and_Barbuda, texts.ct_Argentina, texts.ct_Armenia, texts.ct_Australia,
                            texts.ct_Austria, texts.ct_Azerbaijan, texts.ct_Bahamas, texts.ct_Bahrain, texts.ct_Bangladesh, texts.ct_Barbados, texts.ct_Belarus, texts.ct_Belgium, texts.ct_Belize, texts.ct_Benin, texts.ct_Bermuda, texts.ct_Bhutan,
                            texts.ct_Bolivia, texts.ct_Bosnia_and_Herzegovina, texts.ct_Botswana, texts.ct_Brazil, texts.ct_Brunei, texts.ct_Bulgaria, texts.ct_Burkina_Faso, texts.ct_Burma, texts.ct_Burundi, texts.ct_Cambodia, texts.ct_Cameroon,
                            texts.ct_Canada, texts.ct_Cape_Verde, texts.ct_Central_African_Republic, texts.ct_Chad, texts.ct_Chile, texts.ct_China, texts.ct_Colombia, texts.ct_Comoros, texts.ct_Congo_Democratic_Republic,
                            texts.ct_Congo_Republic_of_the, texts.ct_Costa_Rica, texts.ct_Cote_dIvoire, texts.ct_Croatia, texts.ct_Cuba, texts.ct_Cyprus, texts.ct_Czech_Republic, texts.ct_Denmark, texts.ct_Djibouti, texts.ct_Dominica,
                            texts.ct_Dominican_Republic, texts.ct_East_Timor, texts.ct_Ecuador, texts.ct_Egypt, texts.ct_El_Salvador, texts.ct_Equatorial_Guinea, texts.ct_Eritrea, texts.ct_Estonia, texts.ct_Ethiopia, texts.ct_Fiji, texts.ct_Finland,
                            texts.ct_France, texts.ct_Gabon, texts.ct_Gambia, texts.ct_Georgia, texts.ct_Germany, texts.ct_Ghana, texts.ct_Greece, texts.ct_Greenland, texts.ct_Grenada, texts.ct_Guatemala, texts.ct_Guinea, texts.ct_Guinea_Bissau,
                            texts.ct_Guyana, texts.ct_Haiti, texts.ct_Honduras, texts.ct_Hong_Kong, texts.ct_Hungary, texts.ct_Iceland, texts.ct_India, texts.ct_Indonesia, texts.ct_Iran, texts.ct_Iraq, texts.ct_Ireland, texts.ct_Israel, texts.ct_Italy,
                            texts.ct_Jamaica, texts.ct_Japan, texts.ct_Jordan, texts.ct_Kazakhstan, texts.ct_Kenya, texts.ct_Kiribati, texts.ct_Korea_North, texts.ct_Korea_South, texts.ct_Kuwait, texts.ct_Kyrgyzstan, texts.ct_Laos, texts.ct_Latvia,
                            texts.ct_Lebanon, texts.ct_Lesotho, texts.ct_Liberia, texts.ct_Libya, texts.ct_Liechtenstein, texts.ct_Lithuania, texts.ct_Luxembourg, texts.ct_Macedonia, texts.ct_Madagascar, texts.ct_Malawi, texts.ct_Malaysia,
                            texts.ct_Maldives, texts.ct_Mali, texts.ct_Malta, texts.ct_Marshall_Islands, texts.ct_Mauritania, texts.ct_Mauritius, texts.ct_Mexico, texts.ct_Micronesia, texts.ct_Moldova, texts.ct_Monaco, texts.ct_Mongolia, texts.ct_Morocco,
                            texts.ct_Mozambique, texts.ct_Namibia, texts.ct_Nauru, texts.ct_Nepal, texts.ct_Netherlands, texts.ct_New_Zealand, texts.ct_Nicaragua, texts.ct_Niger, texts.ct_Nigeria, texts.ct_Norway, texts.ct_Oman, texts.ct_Pakistan,
                            texts.ct_Panama, texts.ct_Papua_New_Guinea, texts.ct_Paraguay, texts.ct_Peru, texts.ct_Philippines, texts.ct_Poland, texts.ct_Portugal, texts.ct_Qatar, texts.ct_Romania, texts.ct_Russia, texts.ct_Rwanda, texts.ct_Samoa,
                            texts.ct_San_Marino, texts.ct_Sao_Tome, texts.ct_Saudi_Arabia, texts.ct_Senegal, texts.ct_Serbia_and_Montenegro, texts.ct_Seychelles, texts.ct_Sierra_Leone, texts.ct_Singapore, texts.ct_Slovakia, texts.ct_Slovenia,
                            texts.ct_Solomon_Islands, texts.ct_Somalia, texts.ct_South_Africa, texts.ct_Spain, texts.ct_Sri_Lanka, texts.ct_Sudan, texts.ct_Suriname, texts.ct_Swaziland, texts.ct_Sweden, texts.ct_Switzerland, texts.ct_Syria,
                            texts.ct_Taiwan, texts.ct_Tajikistan, texts.ct_Tanzania, texts.ct_Thailand, texts.ct_Togo, texts.ct_Tonga, texts.ct_Trinidad_and_Tobago, texts.ct_Tunisia, texts.ct_Turkey, texts.ct_Turkmenistan, texts.ct_Uganda, texts.ct_Ukraine,
                            texts.ct_United_Arab_Emirates, texts.ct_United_Kingdom, texts.ct_United_States, texts.ct_Uruguay, texts.ct_Uzbekistan, texts.ct_Vanuatu, texts.ct_Venezuela, texts.ct_Vietnam, texts.ct_Yemen, texts.ct_Zambia, texts.ct_Zimbabwe];

                        months = [texts.m_January, texts.m_February, texts.m_March, texts.m_April, texts.m_May, texts.m_June, texts.m_July, texts.m_August, texts.m_September, texts.m_October, texts.m_November, texts.m_December];

                        $('#id_month').text(months[parseInt($('#id_month').attr('accesskey')) - 1]);

                        textsave = texts.Apply;
                        textcancel = texts.Cancel;
                        male = texts.Male;
                        female = texts.Female;
                        newpassword = texts.ph_New_password;
                        confirmpassword = texts.ph_Confirm_password;
                    }
                },
            });
        }
    });
}