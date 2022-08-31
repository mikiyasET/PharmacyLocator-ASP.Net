const medicineTab = $('#medicineTab')
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
    let path = "admin/";
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

function remove(to, id) {
    let path = "admin/";
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
