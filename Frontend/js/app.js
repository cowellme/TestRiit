$(function () {
    "use strict";

    var API_URL = "http://localhost:8082/api/technic";
    var TYPES_URL = "http://localhost:8082/api/technicType";

    function request(url, options) {
        return $.ajax($.extend({ url: url, dataType: "json" }, options))
            .fail(function (xhr) {
                var message = xhr.responseJSON
                    ? (xhr.responseJSON.title || xhr.responseJSON.detail || JSON.stringify(xhr.responseJSON))
                    : xhr.statusText;
                DevExpress.ui.notify(message || "Ошибка запроса к серверу", "error", 5000);
            });
    }

    var typesStore = new DevExpress.data.CustomStore({
        key: "id",
        loadMode: "raw",
        load: function () {
            return request(TYPES_URL);
        },
        insert: function (values) {
            return request(TYPES_URL, {
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify(toPayload(values))
            })
        },
        update: function (key, values) {
            return request(TYPES_URL + "/" + encodeURIComponent(key), {
                method: "PUT",
                contentType: "application/json",
                data: JSON.stringify(toPayload(values, key))
            });
        },
        remove: function (key) {
            return request(TYPES_URL + "/" + encodeURIComponent(key), {
                method: "DELETE"
            });
        }
    });

    function toPayload(values, key) {
        var payload = {
            accountNumber: values.accountNumber,
            name: values.name,
            technicTypeId: values.technicTypeId,
            roomNumber: values.roomNumber
        };

        if (key != null) {
            payload.id = key;
        }

        return payload;
    }

    var technicStore = new DevExpress.data.CustomStore({
        key: "id",
        loadMode: "raw",
        load: function () {
            return request(API_URL);
        },
        insert: function (values) {
            return request(API_URL, {
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify(toPayload(values))
            });
        },
        update: function (key, values) {
            return request(API_URL + "/" + encodeURIComponent(key), {
                method: "PUT",
                contentType: "application/json",
                data: JSON.stringify(toPayload(values, key))
            });
        },
        remove: function (key) {
            return request(API_URL + "/" + encodeURIComponent(key), {
                method: "DELETE"
            });
        }
    });

    $("#gridTechnicContainer").dxDataGrid({
        dataSource: technicStore,
        keyExpr: "id",
        showBorders: true,
        columnAutoWidth: true,
        remoteOperations: false,
        width: "100%",

        sorting: { mode: "multiple" },
        paging: { pageSize: 10 },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 20, 50],
            showInfo: true
        },
        searchPanel: { visible: true, placeholder: "Поиск..." },
        headerFilter: { visible: true },

        columns: [
            {
                dataField: "accountNumber",
                caption: "Учётный номер",
                sortOrder: "asc"
            },
            {
                dataField: "technicTypeId",
                caption: "Тип техники",
                lookup: {
                    dataSource: typesStore,
                    valueExpr: "id",
                    displayExpr: "name"
                }
            },
            {
                dataField: "name",
                caption: "Наименование"
            },
            {
                dataField: "roomNumber",
                caption: "Комната размещения",
                alignment: "center"
            },
            {
                type: "buttons",
                width: 90,
                buttons: ["edit", "delete"]
            }
        ],

        toolbar: {
            items: [
                { name: "addRowButton", showText: "always", location: "after" },
                { name: "columnChooserButton", location: "after" }
            ]
        },

        editing: {
            mode: "popup",
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            useIcons: true,
            texts: {
                addRow: "Добавить",
                editRow: "Изменить",
                saveRowChanges: "Сохранить",
                cancelRowChanges: "Отмена",
                deleteRowChanges: "Удвалить",
            },
            popup: {
                title: "Карточка техники",
                showTitle: true,
                width: 600,
                height: "auto"
            },
            form: {
                colCount: 2,
                items: [
                    {
                        dataField: "accountNumber",
                        validationRules: [
                            { type: "required", message: "Укажите учётный номер" },
                            { type: "pattern", pattern: /^[a-zA-Z0-9]+$/, message: "Только цифры и латинские буквы" },
                            { type: "stringLength", max: 32, message: "Не более 32 символов" }
                        ]
                    },
                    {
                        dataField: "roomNumber",
                        editorType: "dxNumberBox",
                        editorOptions: {
                            min: 1,
                            max: 1000,
                            showSpinButtons: true
                        },
                        validationRules: [
                            { type: "required", message: "Укажите комнату" },
                            { type: "range", min: 1, max: 1000, message: "От 1 до 1000" }
                        ]
                    },
                    {
                        dataField: "name",
                        colSpan: 2,
                        validationRules: [
                            { type: "required", message: "Укажите наименование" },
                            { type: "stringLength", max: 256, message: "Не более 256 символов" }
                        ]
                    },
                    {
                        dataField: "technicTypeId",
                        colSpan: 2,
                        editorType: "dxSelectBox",
                        editorOptions: {
                            dataSource: typesStore,
                            valueExpr: "id",
                            displayExpr: "name",
                            searchEnabled: true,
                            placeholder: "Выберите тип техники"
                        },
                        validationRules: [
                            { type: "required", message: "Укажите тип техники" }
                        ]
                    }
                ]
            }
        },

        onRowDblClick: function (e) {
            e.component.editRow(e.rowIndex);
        }
    });


    $("#gridTechnicTypeContainer").dxDataGrid({
        dataSource: typesStore,
        keyExpr: "id",
        showBorders: true,
        columnAutoWidth: true,
        remoteOperations: false,
        width: "100%",

        sorting: { mode: "multiple" },
        paging: { pageSize: 10 },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 20, 50],
            showInfo: true
        },
        searchPanel: { visible: true, placeholder: "Поиск..." },
        headerFilter: { visible: true },

        columns: [
            {
                dataField: "id",
                caption: "ID",
                sortOrder: "asc"
            },
            {
                dataField: "name",
                caption: "Наименование"
            },
            {
                type: "buttons",
                width: 90,
                buttons: ["edit", "delete"]
            }
        ],

        toolbar: {
            items: [
                { name: "addRowButton", showText: "always", location: "after" }
            ]
        },

        editing: {
            mode: "popup",
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            useIcons: true,
            texts: {
                addRow: "Добавить",
                editRow: "Изменить",
                saveRowChanges: "Сохранить",
                cancelRowChanges: "Отмена",
                deleteRowChanges: "Удалить"
            },
            popup: {
                title: "Карточка типа",
                showTitle: true,
                width: 600,
                height: "auto"
            },
            form: {
                colCount: 2,
                items: [
                    {
                        dataField: "name",
                        colSpan: 2,
                        validationRules: [
                            { type: "required", message: "Укажите наименование" },
                            { type: "stringLength", max: 256, message: "Не более 256 символов" }
                        ]
                    }
                ]
            }
        },

        onRowDblClick: function (e) {
            e.component.editRow(e.rowIndex);
        }
    });

});
