var API_BASE = window.location.protocol === "file:" ? "http://localhost:8080" : "";

window.App = {
    API_URL: API_BASE + "/api/technic",
    TYPES_URL: API_BASE + "/api/technicType",

    request: function (url, options) {
        return $.ajax($.extend({ url: url, dataType: "json" }, options))
            .fail(function (xhr) {
                var msg = xhr.responseJSON
                    ? (xhr.responseJSON.title || xhr.responseJSON.detail || JSON.stringify(xhr.responseJSON))
                    : xhr.statusText;
                console.error("Ошибка запроса " + url + ":", msg, xhr);
                alert("Ошибка запроса к серверу: " + (msg || "неизвестно"));
            });
    },

    buildRow: function (cells) {
        var $tr = $("<tr>");

        cells.forEach(function (value) {
            $tr.append($("<td>").text(value == null ? "" : value));
        });

        return $tr;
    },

    showEmpty: function ($tbody, colCount, text) {
        $tbody.append(
            $("<tr>").addClass("empty").append(
                $("<td>").attr("colspan", colCount).text(text || "Нет данных")
            )
        );
    },

    typeName: function (typesMap, row) {
        return typesMap[row.technicTypeId] != null
            ? typesMap[row.technicTypeId]
            : row.technicTypeId;
    }
};
