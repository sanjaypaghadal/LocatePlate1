$("#AddDynamicRow").click(function () {
    $("#RowArea").append(`<tr class="mb-4">
                    <td class="w-35">
                        <select name="Section" data-sequence="1" class="form-control form-control-lg" asp-items="@ViewBag.SectionDropDown">
                        </select>
                    </td>
                    <td class="w-65 px-4">
                        <div class="bg-primary p-4">
                        </div>
                    </td>
                    <td>
                        <i class="icon-close text-primary font-30 remCF"></i>
                    </td>
                </tr>`);
    $("#RowArea").on('click', '.remCF', function () {
        $(this).parent().parent().remove();
    });
});