$(function () {
    $("#download-list-of-books").click(function () {
        $.ajax({
            url: "api/Book",
            method: "GET",
            success: function (data) {
                $("#list-container").text("");
                for (idx in data) {
                    $("#list-container").append($("<div>", { text: data[idx].id + " " + data[idx].title + " " + data[idx].author + " " + data[idx].description }));
                }
            }
        })
    });

    $("#post-book").click(function () {
        $.ajax({
            url: "api/Book",
            method: "POST",
            data: JSON.stringify({
                "Title": $("#input-title").val(),
                "Author": $("#input-author").val(),
                "Description": $("#input-description").val()
            }),
            headers: {
                "Content-Type": "application/json",
                "ApiKey": $("#input-api-key").val()
            },
            success: function () {
                $("#input-title").val("");
                $("#input-author").val("");
                $("#input-description").val("");
            }
        })
    });

    $("#delete-book").click(function () {
        $.ajax({
            url: "api/Book/" + $("#input-id").val(),
            method: "DELETE",
            data: $("#input-id").val(),
            headers: {
                "Content-Type": "application/json",
                "ApiKey": $("#input-api-key2").val()
            },
            success: function () {
                $("#input-id").val("");
                $("#input-api-key2").val("");
            }
        })        
    })
});