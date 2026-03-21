var dataTable;

$(document).ready(function(){
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable( {
       "ajax": {url:'/Admin/LoanApplication/GetAll'},
        "columns" : [
        { data: 'id', "width": "5%"},
        { data: 'applicantName', "width": "12%",
            "render": function(data){
                return data ? data : '<span class="text-muted">N/A</span>';
            }
        },
        { data: 'loanAmount', "width": "10%",
            "render": function(data){
                return '$' + parseFloat(data).toLocaleString(undefined, {minimumFractionDigits:2, maximumFractionDigits:2});
            }
        },
        { data: 'loanPurpose', "width": "18%" },
        { data: 'durationMonths', "width": "8%",
            "render": function(data){ return data + ' months'; }
        },
        { data: 'interestRate', "width": "8%",
            "render": function(data){ return data + '%'; }
        },
        { data: 'status', "width": "10%",
            "render": function(data){
                if(data === 'Pending') return '<span class="badge bg-warning text-dark"><i class="bi bi-hourglass-split me-1"></i>Pending</span>';
                if(data === 'Documents Requested') return '<span class="badge bg-info"><i class="bi bi-file-earmark-arrow-up me-1"></i>Docs Requested</span>';
                if(data === 'Approved') return '<span class="badge bg-success"><i class="bi bi-check-circle me-1"></i>Approved</span>';
                if(data === 'Rejected') return '<span class="badge bg-danger"><i class="bi bi-x-circle me-1"></i>Rejected</span>';
                return '<span class="badge bg-secondary">' + data + '</span>';
            }
        },
        { data: 'createdAt', "width": "12%",
            "render": function(data){
                var d = new Date(data);
                return d.toLocaleDateString('en-GB', {day:'2-digit', month:'short', year:'numeric'});
            }
        },
        { data: 'id',
        "render": function(data){
            return `<a href="/Admin/LoanApplication/Details/${data}" class="btn btn-sm btn-outline-primary" title="View Details"><i class="bi bi-eye"></i> Review</a>`;
        },
            "width": "10%" }
    
    ]});    
}
