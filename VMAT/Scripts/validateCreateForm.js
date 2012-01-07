// File: validateCreateForm.js

$(document).ready(function () {
    $("#CreateForm").validate({
        rules: {
            "project-menu-field": {
                required: true,
                digits: true,
                minlength: 4,
                maxlength: 4
            },

            "machine-suffix": {
                required: true,
                minlength: 1,
                maxlength: 5
            },

            "image-menu": {
                required: true,
                accept: "vmx"
            }
        },

        messages: {
            "project-menu-field": {
                required: "Please select a project number.",
                digits: "",
                minlength: "",
                maxlength: ""
            },

            "machine-suffix": {
                required: "Please enter a machine suffix.",
                minlength: "",
                maxlength: ""
            },

            "image-menu": {
                required: "Please select an image file.",
                accept: jQuery.format("Image file must be of type '{0}'.")
            }
        }
    });
});