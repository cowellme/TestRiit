$(function () {
    "use strict";

    var typesMap = {};
    var allRows = [];

    function fillTypeFilter(types) {
        var $select = $("#filterType");
        $select.find("option:not(:first)").remove();

        types
            .slice()
            .sort(function (a, b) { return a.name.localeCompare(b.name, "ru"); })
            .forEach(function (t) {
                $select.append($("<option>").val(t.id).text(t.name));
            });
    }

    function fillRoomFilter(rows) {
        var $select = $("#filterRoom");
        $select.find("option:not(:first)").remove();

        var rooms = {};

        rows.forEach(function (r) {
            if (r.roomNumber != null) {
                rooms[r.roomNumber] = true;
            }
        });

        Object.keys(rooms)
            .map(Number)
            .sort(function (a, b) { return a - b; })
            .forEach(function (room) {
                $select.append($("<option>").val(room).text(room));
            });
    }

    function getFilters() {
        return {
            typeId: $("#filterType").val(),
            room: $("#filterRoom").val(),
            search: $.trim($("#filterSearch").val()).toLowerCase()
        };
    }

    function applyFilters() {
        var f = getFilters();

        var filtered = allRows.filter(function (r) {
            if (f.typeId !== "" && String(r.technicTypeId) !== f.typeId) {
                return false;
            }

            if (f.room !== "" && String(r.roomNumber) !== f.room) {
                return false;
            }

            if (f.search) {
                var haystack = [r.accountNumber, r.name].join(" ").toLowerCase();

                if (haystack.indexOf(f.search) === -1) {
                    return false;
                }
            }

            return true;
        });

        renderTable(filtered);
    }

    function renderTable(rows) {
        var $tbody = $("#filteredTable tbody").empty();

        if (!rows.length) {
            App.showEmpty($tbody, 5, "По выбранным фильтрам ничего не найдено");
            return;
        }

        rows.forEach(function (r) {
            $tbody.append(App.buildRow([
                r.id,
                r.accountNumber,
                r.name,
                App.typeName(typesMap, r),
                r.roomNumber
            ]));
        });
    }

    function resetFilters() {
        $("#filterType").val("");
        $("#filterRoom").val("");
        $("#filterSearch").val("");
        applyFilters();
    }

    $("#filterType").on("change", applyFilters);
    $("#filterRoom").on("change", applyFilters);
    $("#filterSearch").on("input", applyFilters);
    $("#resetFilters").on("click", resetFilters);

    $.when(
        App.request(App.TYPES_URL),
        App.request(App.API_URL)
    ).done(function (typesResult, technicResult) {
        var types = typesResult[0] || [];
        var technic = technicResult[0] || [];

        types.forEach(function (t) {
            typesMap[t.id] = t.name;
        });

        allRows = technic;

        fillTypeFilter(types);
        fillRoomFilter(allRows);
        applyFilters();
    });
});
