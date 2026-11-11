$(function () {
    "use strict";

    var typesMap = {};
    var allRows = [];
    var editingId = null;
    var sortState = { field: "account", dir: 1 };

    function fillTypeSelect() {
        var $select = $("#editType").empty();
        $select.append($("<option>").val("").text("Выберите тип"));

        Object.keys(typesMap)
            .map(Number)
            .sort(function (a, b) {
                return typesMap[a].localeCompare(typesMap[b], "ru");
            })
            .forEach(function (id) {
                $select.append($("<option>").val(id).text(typesMap[id]));
            });
    }

    function renderTypes(types) {
        var $tbody = $("#typesTable tbody").empty();

        if (!types || !types.length) {
            App.showEmpty($tbody, 2);
            return;
        }

        types.forEach(function (t) {
            $tbody.append(App.buildRow([t.id, t.name]));
        });
    }

    function sortValue(row, field) {
        if (field === "type") {
            return String(App.typeName(typesMap, row)).toLowerCase();
        }

        if (field === "room") {
            return Number(row.roomNumber);
        }

        return String(row[field] || "").toLowerCase();
    }

    function updateSortIndicators() {
        $("#technicTable th[data-sort]").each(function () {
            var $th = $(this);
            $th.removeClass("sorted-asc sorted-desc");

            if ($th.attr("data-sort") === sortState.field) {
                $th.addClass(sortState.dir === 1 ? "sorted-asc" : "sorted-desc");
            }
        });
    }

    function renderTechnic() {
        var $tbody = $("#technicTable tbody").empty();

        var rows = allRows.slice().sort(function (a, b) {
            var va = sortValue(a, sortState.field);
            var vb = sortValue(b, sortState.field);

            if (va < vb) return -1 * sortState.dir;
            if (va > vb) return 1 * sortState.dir;
            return 0;
        });

        if (!rows.length) {
            App.showEmpty($tbody, 5, "Нет данных");
            return;
        }

        rows.forEach(function (r) {
            var $tr = App.buildRow([
                r.accountNumber,
                App.typeName(typesMap, r),
                r.name,
                r.roomNumber
            ]);

            var $td = $("<td>").addClass("actions-cell");

            $("<button>")
                .addClass("btn btn-secondary btn-sm")
                .text("Редактировать")
                .on("click", function () {
                    openEditDialog(r);
                })
                .appendTo($td);

            $tr.append($td);
            $tr.on("dblclick", function () {
                openEditDialog(r);
            });
            $tbody.append($tr);
        });
    }

    function loadAll() {
        $.when(
            App.request(App.TYPES_URL),
            App.request(App.API_URL)
        ).done(function (typesResult, technicResult) {
            var types = typesResult[0] || [];

            typesMap = {};
            types.forEach(function (t) {
                typesMap[t.id] = t.name;
            });

            allRows = technicResult[0] || [];

            renderTypes(types);
            fillTypeSelect();
            renderTechnic();
            updateSortIndicators();
        });
    }

    function openAddDialog() {
        editingId = null;
        $("#dialogTitle").text("Добавление техники");
        $("#technicEditForm")[0].reset();
        $("#technicDialog").prop("hidden", false);
    }

    function openEditDialog(row) {
        editingId = row.id;
        $("#dialogTitle").text("Редактирование техники №" + row.id);
        $("#editAccount").val(row.accountNumber);
        $("#editName").val(row.name);
        $("#editType").val(row.technicTypeId);
        $("#editRoom").val(row.roomNumber);
        $("#technicDialog").prop("hidden", false);
    }

    function closeDialog() {
        $("#technicDialog").prop("hidden", true);
    }

    function validateDialog() {
        var account = $.trim($("#editAccount").val());
        var name = $.trim($("#editName").val());
        var typeId = $("#editType").val();
        var room = $.trim($("#editRoom").val());

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
            alert("Комната размещения: целое число от 1 до 1000");
            return false;
        }

        return true;
    }

    function saveTechnic() {
        if (!validateDialog()) {
            return;
        }

        var payload = {
            accountNumber: $.trim($("#editAccount").val()),
            name: $.trim($("#editName").val()),
            technicTypeId: Number($("#editType").val()),
            roomNumber: Number($("#editRoom").val())
        };

        if (editingId != null) {
            payload.id = editingId;
        }

        var url = editingId == null
            ? App.API_URL
            : App.API_URL + "/" + encodeURIComponent(editingId);

        var method = editingId == null ? "POST" : "PUT";

        App.request(url, {
            method: method,
            contentType: "application/json",
            data: JSON.stringify(payload)
        }).done(function () {
            closeDialog();
            loadAll();
        });
    }

    $("#technicTable").on("click", "th[data-sort]", function () {
        var field = $(this).attr("data-sort");

        if (sortState.field === field) {
            sortState.dir = -sortState.dir;
        } else {
            sortState.field = field;
            sortState.dir = 1;
        }

        renderTechnic();
        updateSortIndicators();
    });

    $("#technicEditForm").on("submit", function (e) {
        e.preventDefault();
    });

    $("#addTechnicBtn").on("click", openAddDialog);
    $("#saveTechnic").on("click", saveTechnic);
    $("#cancelDialog").on("click", closeDialog);

    $("#technicDialog").on("click", function (e) {
        if (e.target === this) {
            closeDialog();
        }
    });

    loadAll();
});
