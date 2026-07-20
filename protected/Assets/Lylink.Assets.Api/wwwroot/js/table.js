document.addEventListener("DOMContentLoaded", function () {
    const tables = document.querySelectorAll("table");
    tables.forEach(table => {
        const headers = Array.from(table.querySelectorAll("thead th")).map(th => th.innerText);
        table.querySelectorAll("tbody tr").forEach(row => {
            row.querySelectorAll("td").forEach((cell, index) => {
                cell.setAttribute("data-header", headers[index]);
            });
        });
    });
});