var urlroute = location.protocol + '//' + location.host;
var calendar;



document.addEventListener('DOMContentLoaded', function () {




    $('#appointingdate').kendoDateTimePicker({
        value: new Date(),
        dataInput: false
    })


    rendercalendar();

});

function OnopeningModal(obj, isDetailevent) {

    if (isDetailevent != null) {
        $('#title').val(obj.title);
        $('#description').val(obj.description);
        $('#appointingdate').val(obj.startDate);
        $('#duration').val(obj.duration);
        $('#patientid').val(obj.Patient_Id);
        $('#doctorId').val(obj.dctor_Id);
        $('#Id').val(obj.id);
        $('#lbldoctorName').html(obj.doctorName);
        $('#lblPatietName').html(obj.patientName);


        $('#btnDelete').addClass('d-block');

        if (obj.doctorApproved)
           {
            $('#lblstatus').html('Approved');
            $('#btnConfirm').addClass('d-none');
            $('#btnSubmit').addClass('d-none');
           }
        else
          {
            $('#lblstatus').html('pendding');
            $('#btnConfirm').addClass('d-block');
            $('#btnSubmit').addClass('d-block');
          }
    }
    else
    {
        $('#btnDelete').addClass('d-none');
    }
    $('#appointmentInput').modal('show');
}


function closemodal() {
    $('#appointmentInput').modal('hide');
}

function onSubmitForm() {
    var requestdata = {
        Id: parseInt($('#id').val()),
        Title: $('#title').val(),
        Description: $('#description').val(),
        StartDate: $('#appointingdate').val(),
        Doctor_Id: $('#doctorId').val(),
        Patient_Id: $('#patientid').val(),
        Duration: $('#duration').val(),
    }


    if (CheckValidation()) {
        $.ajax({
            url: urlroute + '/api/Appointment/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestdata),
            contentType: 'application/json',
            success: function (response) {
                if (response.status == 1) {
                    rendercalendar();
                    $.notify(response.message, "success");
                    closemodal();
                }
                else
                    $.notify(response.message, "error")
            },
            error: function (xhr) {
                $.notify("Error", "error")
            }
        })
    }

}


function CheckValidation() {

    var isValid = true;

    if ($('#title').val() === "" || $('#title').val() === undefined) {
        isvalid = false;
        $('#title').addClass('error')
    }
    else {
        $('#title').removeClass('error')
        isvalid = true;
    }


    if ($('#appointingdate').val() === "" || $('#appointingdate').val() === undefined) {
        isvalid = false;
        $('#appointingdate').addClass('error')
    }
    else {
        $('#appointingdate').removeClass('error')
        isvalid = true;
    }

    return isvalid;

}



function GetEventDetailsById(info) {
    $.ajax({
        url: urlroute + '/api/Appointment/GetCalendarDataById/' + info.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status == 650 && response.dataenum != undefined) {
                OnopeningModal(response.dataenum, true)

            }
            else
                $.notify(response.message, "error")
        },
        error: function (xhr) {
            $.notify("Error", "error")
        }
    })
}



function onDoctorChnage() {
    rendercalendar();
}


function rendercalendar() {
    calendar = document.getElementById('calendar');

    if (calendar != null) {

        var calendar = new FullCalendar.Calendar(calendar, {
            headerToolbar: { center: 'dayGridMonth,timeGridWeek', start: 'title', end: 'today prev,next' }, // buttons for switching between views
            views: {
                dayGridMonth: { // name of view
                    titleFormat: { year: 'numeric', month: '2-digit', day: '2-digit' }
                    // other view-specific options here
                }
            },
            dateClick: function () {
                OnopeningModal(null, null);
            },
            eventDisplay: 'block',
            events: function (fetchinfo, successCallback, failuerCallback) {
                $.ajax({
                    url: urlroute + '/api/Appointment/GetCalendarData?doctorId=' + $('#doctorId').val(),
                    type: 'GET',

                    datatype: 'JSON',
                    success: function (response) {
                        var datas = [];

                        if (response.status === 650) {
                            $.each(response.dataenum, function (i, data) {
                                datas.push({
                                    title: data.title,
                                    description: data.description,
                                    start: data.startDate,
                                    end: data.endDate,
                                    backgroundcolor: data.doctorApproved ? "#28ea745" : "#dc3545",
                                    textColor: "white",
                                    id: data.id
                                })
                            });
                        }
                        successCallback(datas);

                    },
                    error: function (xhr) {
                        $.notify("Error", "error")
                    }
                })
            },
            eventClick: function (info) {
                GetEventDetailsById(info.event);
            }
        });

    }

    calendar.render();
}

function onDeleteAppointment() {

    var id = $('#Id').val();

    console.log(urlroute)

    $.ajax({
        url: urlroute +'/api/Appointment/DeleteData/'+id,
        contentType :'application/json',
        dataType:'JSON',
        method: 'POST',
        success: function (response) {
            if (response.status === 650) {
                alert(response.message);
                rendercalendar();
            }
            else
                alert(response.message);
        },
        error: function (response) {
            alert(response.error);
        }
    })
}


function onConfirmAppointment(){
    $.ajax({
        url: urlroute + '/api/Appointment/ConfrimAppointment/'+ $('#Id').val(),
        type: 'GET',
        datatype: 'JSON',
        success: function (response) {
            var datas = [];

            if (response.status === 650) {
                closemodal();
                rendercalendar();
            } 
        },
        error: function (xhr) {
            $.notify("Error", "error")
        }
    })
}