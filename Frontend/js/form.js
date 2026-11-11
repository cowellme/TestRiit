$(function () {
    "use strict";

    var typesMap = {};

    function fillTypeSelect(types) {
        var $select = $("#formType").empty();
        $select.append($("<option>").val("").text("Выберите тип"));

        types
            .slice()
            .sort(function (a, b) { return a.name.localeCompare(b.name, "ru"); })
            .forEach(function (t) {
                $select.append($("<option>").val(t.id).text(t.name));
            });
    }

    function renderTable(rows) {
        var $tbody = $("#formTable tbody").empty();

        if (!rows.length) {
            App.showEmpty($tbody, 6, "Техника не добавлена");
            return;
        }

        rows.forEach(function (r) {
            var $tr = App.buildRow([
                r.id,
                r.accountNumber,
                r.name,
                App.typeName(typesMap, r),
                r.roomNumber
            ]);

            var $td = $("<td>").addClass("actions-cell");

            $("<button>")
                .addClass("btn btn-danger")
                .text("Удалить")
                .on("click", function () {
                    deleteTechnic(r.id);
                })
                .appendTo($td);

            $tr.append($td);
            $tbody.append($tr);
        });
    }

    function loadAll() {
        $.when(
            App.request(App.TYPES_URL),
            App.request(App.API_URL)
        ).done(function (typesResult, technicResult) {
            var types = typesResult[0] || [];
            var technic = technicResult[0] || [];

            typesMap = {};
            types.forEach(function (t) {
                typesMap[t.id] = t.name;
            });

            fillTypeSelect(types);
            renderTable(technic);
        });
    }

    function deleteTechnic(id) {
        if (!window.confirm("Удалить технику №" + id + "?")) {
            return;
        }

        App.request(App.API_URL + "/" + encodeURIComponent(id), { method: "DELETE" })
            .done(loadAll);
    }

    function validate() {
        var account = $.trim($("#formAccount").val());
        var name = $.trim($("#formName").val());
        var typeId = $("#formType").val();
        var room = $.trim($("#formRoom").val());

        if (!/^[a-zA-Z0-9]{1,32}$/.test(account)) {
            alert("Учётный номер: только цифры и латинские буквы, до 32 символов");
            return false;
        }

        if (!name || name.length > 256) {
            alert("Укажите наименование (до 256 символов)");
            return false;
        }

        if (!typeId) {
            alert("Выберите тип техники");
            return false;
        }

        if (!/^\d+$/.test(room) || +room < 1 || +room > 1000) {
            alert("Комната: целое число от 1 до 1000");
            return false;
        }

        return true;
    }

    $("#technicForm").on("submit", function (e) {
        e.preventDefault();
    });

    $("#addTechnic").on("click", function () {
        if (!validate()) {
            return;
        }

        var payload = {
            accountNumber: $.trim($("#formAccount").val()),
            name: $.trim($("#formName").val()),
            technicTypeId: Number($("#formType").val()),
            roomNumber: Number($("#formRoom").val())
        };

        App.request(App.API_URL, {
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(payload)
        }).done(function () {
            $("#technicForm")[0].reset();
            loadAll();
        });
    });

    $("#resetForm").on("click", function () {
        $("#technicForm")[0].reset();
    });

    // TODO: добавление и удаление типов техники из справочника требует
    // новых методов POST/DELETE в TechnicTypeController на бэкенде
    loadAll();
});
