﻿const medicineTab = $('#medicineTab')
const locationTab = $('#locationTab')
const pharmacyTab = $('#pharmacyTab')
const storeTab = $('#storeTab')

const medicineLink = $('#medicineLink')
const locationLink = $('#locationLink')
const pharmacyLink = $('#pharmacyLink')
const storeLink = $('#storeLink')

const medicineIcon = $('#medicineLink i:last-child')
const locationIcon = $('#locationLink i:last-child')
const pharmacyIcon = $('#pharmacyLink i:last-child')
const storeIcon = $('#storeLink i:last-child')
const path = "admin/";

medicineTab.hide()
locationTab.hide()
pharmacyTab.hide()
storeTab.hide()

const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
})

function loadPage(location, data = '', id = '') {
    let request;
    switch (location) {
        case 'dashboard':
            console.log("Dashboard")
            request = $.ajax({
                url: path + "dashboard",
                type: "GET",
                dataType: "html",
                data: {
                    func: data,
                    id: id
                }
            });
            break;
        case 'medicine':
            request = $.ajax({
                url: path + "medicine",
                type: "GET",
                dataType: "html",
                data: {
                    func: data,
                    id: id
                }
            });
            break;
        case 'location':
            request = $.ajax({
                url: path + "location",
                type: "GET",
                dataType: "html",
                data: {
                    func: data,
                    id: id
                }
            });
            break;
        case 'pharmacy':
            request = $.ajax({
                url: path + "pharmacy",
                type: "GET",
                dataType: "html",
                data: {
                    func: data,
                    id: id
                }
            });
            break;
        case 'store':
            request = $.ajax({
                url: path + "store",
                type: "GET",
                dataType: "html",
                data: {
                    func: data,
                    id: id
                }
            });
            break;
        case 'leaderboard':
            request = $.ajax({
                url: path + "leaderboard",
                type: "GET",
                dataType: "html",
                data: {
                    func: data,
                    id: id
                }
            });
            break;
        case 'password':
            request = $.ajax({
                url: path + "password",
                type: "GET",
                dataType: "html",
            });
            break;
        case 'logout':
            request = $.ajax({
                url: path + "logout",
                type: "GET",
                dataType: "html",
            });
            break;
        default:
            request = $.ajax({
                url: "./Dashboard",
                type: "GET",
                dataType: "html",
                data: {
                    func: data,
                    id: id
                }
            });
    }
    request.done(function (msg) {
        $(".main").html(msg);
        console.log('page changed')
       
    });
    request.fail(function (jqXHR, textStatus) {
        Toast.fire({
            icon: 'error',
            title: 'Connection Error'
        })
    });
}

function add(to) {
    switch (to) {
        case 'medicine':
            medicineBtn()
        case 'location':
            locationBtn()
            break
        default:
            Toast.fire({
                icon: 'error',
                title: 'Unknown call'
            })
    }
}
function modify(to, id) {
    switch (to) {
        case 'medicine':
            medicineBtn('edit', id)
            break
        case 'location':
            locationBtn('edit', id)
            break
        default:
            Toast.fire({
                icon: 'error',
                title: 'Unknown call'
            })
    }
}
function remove(to, id) {
    showLoading()
    let name = ''
    let tab = ''
    let load = ''
    switch (to) {
        case 'medicine':
            tab = 'removeMedicine'
            name = 'medicine'
            load = 'medicine'
            break
        case 'location':
            tab = 'removeLocation'
            name = 'location'
            load = 'location'
            break
        case 'pharmacy':
            tab = 'removePharmacy'
            name = 'pharmacy'
            load = 'pharmacy'
            break
        case 'store':
            tab = 'removeStore'
            name = 'store'
            load = 'store'
            break
        default:
            name = 'unknown thing'
    }
    Swal.fire({
        title: 'Are you sure?',
        text: "You're about to delete this " + name + "!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            let request = $.ajax({
                url: path + "RemoveThem",
                type: "DELETE",
                data: {
                    id: id,
                    submit: tab
                },
                dataType: "text"
            });
            request.done(function (output) {
                console.log(output);
                switch (output) {
                    case 'success':
                        Toast.fire({
                            icon: 'success',
                            title: 'Deleted successfully'
                        })
                        loadPage(load, 'remove')
                        break;
                    case 'failure':
                        Toast.fire({
                            icon: 'error',
                            title: 'Not deleted, try again later!'
                        })
                        break;
                    case 'unknownID':
                        Toast.fire({
                            icon: 'error',
                            title: 'something went wrong, refresh the page!'
                        })
                        break;
                }
                hideLoading()
            });
            request.fail(function (jqXHR, textStatus) {
                Toast.fire({
                    icon: 'error',
                    title: 'Request failed: ' + textStatus
                })
                hideLoading()
            });
        }
        else {
            hideLoading()
        }
    })
}

function medicineBtn(data = 'add', id = null) {
    var formData = new FormData();
    formData.append('Id', id)
    formData.append('Name', $("input[name='name']").val())
    formData.append('Description', $("textarea[name='description']").val())
    formData.append('Image', $("input[name='image']")[0].files[0])
    formData.append('submit', data === 'add' ? 'addMedicine' : 'editMedicine')
    $.ajax({
        url: "admin/ModMedicine",
        type: "POST",
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        beforeSend: function () {
            showLoading()
            $('button[type="submit"]').prop('disabled', true);
        },
        success: function (data) {
            hideLoading()
            $('button[type="submit"]').prop('disabled', false);
            console.log(data)
            switch (data) {
                case 'success':
                    Toast.fire({
                        icon: 'success',
                        title: 'Medicine added successfully'
                    })
                    loadPage('medicine', 'add')
                    break;
                case 'editsuccess':
                    Toast.fire({
                        icon: 'success',
                        title: 'Medicine modified successfully'
                    })
                    loadPage('medicine', 'edit')
                    break;
                case 'nameExist':
                    Toast.fire({
                        icon: 'error',
                        title: 'Name already exists'
                    })
                    break;
                case 'imageError':
                    Toast.fire({
                        icon: 'error',
                        title: 'Please select an image.'
                    })
                    break;
                case 'NameEmpty':
                    Toast.fire({
                        icon: 'error',
                        title: 'Please enter Medicine Name it\'s required.'
                    })
                    break;
                case 'unknownTask':
                    Toast.fire({
                        icon: 'error',
                        title: 'Please refresh the tab and try again.'
                    })
                    break;
                default:
                    Toast.fire({
                        icon: 'error',
                        title: 'Unexpexted Error'
                    })
            }
        },
        error: function (e) {
            hideLoading()
            Toast.fire({
                icon: 'error',
                title: 'Internal Error'
            })
            $('button[type="submit"]').prop('disabled', false);
        }
    });
}
function locationBtn(data = 'add', id = null) {
    showLoading()
    let name = $("input[name='name']").val();
    let request = $.ajax({
        url: path + "ModLocation",
        type: "POST",
        data: {
            id: id,
            name: name,
            submit: data === 'add' ? 'addLocation' : 'editLocation'
        },
        dataType: "text"
    });
    if (name != '') {
        request.done(function (output) {
            console.log(output);
            switch (output) {
                case 'locationSuccess':
                    Toast.fire({
                        icon: 'success',
                        title: 'Location added successfully'
                    })
                    loadPage('location', 'add')
                    break;
                case 'locationNameExist':
                    Toast.fire({
                        icon: 'error',
                        title: 'Name already exists'
                    })
                    break;
                case 'editLocationSuccess':
                    Toast.fire({
                        icon: 'success',
                        title: 'Location modified successfully'
                    })
                    loadPage('location', 'edit')
                    break;
                case 'editLocationUnknownID':
                    Toast.fire({
                        icon: 'error',
                        title: 'Id unknown, Please refresh the page'
                    })
                    break;
                default:
                    Toast.fire({
                        icon: 'error',
                        title: 'Internal Error'
                    })
            }
            hideLoading()
        });
        request.fail(function (jqXHR, textStatus) {
            Toast.fire({
                icon: 'error',
                title: 'Connection Error'
            })
            hideLoading()
        });
    } else {
        Toast.fire({
            icon: 'error',
            title: 'Empty field'
        })
        hideLoading()
    }
}

function showLoading() {
    $('#mainpage').addClass('loading')
}
function hideLoading() {
    $('#mainpage').removeClass('loading')
}


function collapseAll($except = null) {
    switch ($except) {
        case 'medicine':
            pharmacyIcon.text("expand_more")
            locationIcon.text("expand_more")
            storeIcon.text("expand_more")

            pharmacyTab.slideUp()
            locationTab.slideUp()
            storeTab.slideUp()
            break
        case 'location':
            pharmacyIcon.text("expand_more")
            medicineIcon.text("expand_more")
            storeIcon.text("expand_more")

            pharmacyTab.slideUp()
            medicineTab.slideUp()
            storeTab.slideUp()
            break
        case 'pharmacy':
            medicineIcon.text("expand_more")
            locationIcon.text("expand_more")
            storeIcon.text("expand_more")

            medicineTab.slideUp()
            locationTab.slideUp()
            storeTab.slideUp()
            break
        case 'store':
            medicineIcon.text("expand_more")
            locationIcon.text("expand_more")
            pharmacyIcon.text("expand_more")
            medicineTab.slideUp()
            locationTab.slideUp()
            pharmacyTab.slideUp()
            break
        default:
            medicineIcon.text("expand_more")
            locationIcon.text("expand_more")
            pharmacyIcon.text("expand_more")
            storeIcon.text("expand_more")
            medicineTab.slideUp()
            locationTab.slideUp()
            pharmacyTab.slideUp()
            storeTab.slideUp()
            break;
    }
}
var links = document.querySelectorAll('.list-link');

links.forEach((e) => {
    e.addEventListener('click', function () {
        links.forEach((e) => {
            e.classList.remove('active')
        })
        e.className += ' active'
    })
})

$('a').on('click', function (e) {
    const current = e.currentTarget.id;
    switch (current) {
        case 'medicineLink':
            collapseAll('medicine')
            medicineTab.slideToggle()
            medicineIcon.text() === "expand_more" ? medicineIcon.text('expand_less') : medicineIcon.text("expand_more")
            break
        case 'locationLink':
            collapseAll('location')
            locationTab.slideToggle()
            locationIcon.text() === "expand_more" ? locationIcon.text('expand_less') : locationIcon.text("expand_more")
            break
        case 'pharmacyLink':
            collapseAll('pharmacy')
            pharmacyTab.slideToggle()
            pharmacyIcon.text() === "expand_more" ? pharmacyIcon.text('expand_less') : pharmacyIcon.text("expand_more")
            break
        case 'storeLink':
            collapseAll('store')
            storeTab.slideToggle()
            storeIcon.text() === "expand_more" ? storeIcon.text('expand_less') : storeIcon.text("expand_more")
            break
        case 'leaderboardLink':
            collapseAll()
            break
        case 'dashboardLink':
            collapseAll()
            break
        case 'settingsLink':
            collapseAll()
            break
        case 'broadcastLink':
            collapseAll()
            break
    }
})
$('#ca-collapse').on('click', function (e) {
    $('#ca-collapse > i').text() === 'menu' ? $('#ca-collapse > i').text('close') : $('#ca-collapse > i').text('menu')
    $('#aside-menu').toggle('fast')
    $('.logo').toggle('fast')
    $('aside').animate({
        right: 'toggle',
        width: 'toggle'
    }, 'fast', "swing", () => { });
})
$('.main').on('click', function (e) {
    if ($(window).width() < 980) {
        $('#ca-collapse > i').text('menu')
        $('#aside-menu').hide('fast')
        $('.logo').hide('fast')
        $('aside').animate({
            width: 'hide'
        }, 'fast', "swing", () => { });
    }
})