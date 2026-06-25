window.getMisTableConfig = function(customConfig) {
    let baseConfig = {
        "processing": true,
        "serverSide": true,
        "responsive": false,
        "colReorder": true,
        "language": {
            "search": "_INPUT_",
            "searchPlaceholder": "Search...",
            "lengthMenu": "Show _MENU_ entries"
        },
        "dom": "<'row align-items-center mb-3'<'col-sm-12 col-md-4'l><'col-sm-12 col-md-8 d-flex align-items-center justify-content-end gap-2'B>>" +
               "<'row'<'col-sm-12'tr>>" +
               "<'row mt-3'<'col-sm-12 col-md-5 text-secondary'i><'col-sm-12 col-md-7'p>>",
        "buttons": {
            dom: {
                container: { className: 'dt-buttons d-flex gap-2' }
            },
            buttons: [
                {
                    extend: 'colvis',
                    text: '<i class="bi bi-view-list me-1"></i> Custom Listing',
                    className: 'btn shadow-none border px-3 custom-ctrl-height',
                    attr: {
                        style: 'background-color: #f1f5f9; color: #1e293b; border-color: #cbd5e1 !important; border-radius: 50px;'
                    }
                }
            ]
        },
        "initComplete": function(settings, json) {
            $('.dataTables_filter input').addClass('form-control mis-search-input custom-ctrl-height');
            $('.dataTables_length select').addClass('form-select mis-select-input custom-ctrl-height');
            if (customConfig.initComplete) {
                customConfig.initComplete(settings, json);
            }
        }
    };
    
    // Deep merge the baseConfig with customConfig (arrays like buttons will be merged properly or overwritten based on jQuery extend behavior, we might need a custom merge for buttons if we want to add more)
    
    // If the custom config has its own buttons array, we want to combine them!
    if (customConfig.buttons && customConfig.buttons.buttons) {
        var customButtons = customConfig.buttons.buttons;
        // Keep the base Custom Listing button, but add custom ones
        baseConfig.buttons.buttons = customButtons.concat(baseConfig.buttons.buttons);
        delete customConfig.buttons; // prevent deep merge from overwriting
    }

    return $.extend(true, {}, baseConfig, customConfig);
};

window.triggerMisExport = function(tableInstance, format, baseParams, columnNameMap) {
    var title = prompt('Enter a title for your export:', 'Document Export');
    if (title === null) return; // cancelled
    if (!title.trim()) title = 'Document Export';

    // Collect visible column keys
    var visibleCols = [];
    tableInstance.columns(':visible').every(function() {
        var key = columnNameMap[this.index()];
        if (key && key !== 'manage' && key !== 'checkbox') {
            visibleCols.push(key);
        }
    });

    var urlParams = new URLSearchParams(baseParams);
    urlParams.set('format', format);
    urlParams.set('title', title);
    urlParams.set('columns', visibleCols.join(','));

    // Navigate to export URL — browser triggers file download automatically
    window.location.href = '?' + urlParams.toString();
};
