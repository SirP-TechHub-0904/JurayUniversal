$(document).ready(function () {
    jQueryModalGet = (url, title) => {
        try {
            $.ajax({
                type: 'GET',
                url: url,
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#main-form-modal .modal-body').html(res.html);
                    $('#main-form-modal .modal-title').html(title);
                    $('#main-form-modal').modal('show');
                },
                error: function (err) {
                    console.log(err)
                    toastr.error('error')
                }
            })
            return false;
        } catch (ex) {
            console.log(ex)
            toastr.error('error')
        }
    }
    jQueryModalPost = form => { 
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                contentType: false,
                processData: false,
                data: new FormData(form),
               
                success: function (res) {  
                    if (res.isValid) {
                        $('#viewAll').html(res.html) 
                        $('#main-form-modal').modal('hide'); 
                        toastr.success('success')
                    }
                },
                error: function (err) {
                    console.log(err) 
                    toastr.error('error')
                },
                complete: function () {
                    $('#loading-spinner').hide(); // Hide the loading spinner
                }
            })
            return false;
        } catch (ex) {
            console.log(ex)
            toastr.error('error')
        }
    }
    jQueryModalDelete = form => {
        if (confirm('Are you sure to delete this record ?')) {
            try {
                $.ajax({
                    type: 'POST',
                    url: form.action,
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        $('#viewAll').html(res.html);
                        toastr.success('success')
                    },
                    error: function (err) {
                        console.log(err)
                        toastr.error('error')
                    },
                    complete: function () {
                        $('#loading-spinner').hide(); // Hide the loading spinner
                    }
                })
            } catch (ex) {
                console.log(ex)
                toastr.error('error')
            }
        }
        return false;
    }

    jQueryModalRestore = form => {
        if (confirm('Restore record ?')) {
            try {
                $.ajax({
                    type: 'POST',
                    url: form.action,
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        $('#viewAll').html(res.html);
                        toastr.success('success')
                    },
                    error: function (err) {
                        console.log(err)
                        toastr.error('error')
                    }
                })
            } catch (ex) {
                console.log(ex)
                toastr.error('error')
            }
        }
        return false;
    }
});