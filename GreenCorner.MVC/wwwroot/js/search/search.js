$(document).ready(function () {
    let timeout = null;
    const searchInput = $("#search-input-realtime");
    const searchResults = $("#search-results");
    const searchFormContainer = $(".search-style-2");

    function updateDropdownWidth() {
        let searchWidth = searchFormContainer.outerWidth();
        searchResults.css({
            "width": 500 + "px",
            "min-width": 500 + "px"
        });
    }

    updateDropdownWidth();
    $(window).resize(updateDropdownWidth);

    searchInput.on("keyup", function () {
        clearTimeout(timeout);
        const query = $(this).val().trim();

        if (query.length < 1) {
            searchResults.html("").hide();
            return;
        }

        timeout = setTimeout(() => {
            // Hiển thị trạng thái đang tìm kiếm
            displayLoading();

            $.ajax({
                url: `/Product/Search?keyword=${query}`,
                type: "GET",
                success: function (response) {
                    if (response.success && response.products.length > 0) {
                        displayResults(response.products, query);
                    } else {
                        displayNoResult(query);
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching search results:", error);
                    displayNoResult(query, "Đã có lỗi xảy ra. Vui lòng thử lại.");
                }
            });
        }, 300); // Debounce time: 300ms
    });

    function displayLoading() {
        const html = `<li class="search-info-li"><span>Đang tìm kiếm...</span></li>`;
        searchResults.html(html).show();
    }

    function displayResults(products, query) {
        let html = "";
        products.forEach(product => {
            // Giả định API trả về: productId, name, imageUrl, và priceFormatted
            const img = product.imageUrl.split('&')[0];
            const price = product.priceFormatted || ''; // Hiển thị giá nếu có

            html += `
                <li class="search-result-item">
                    <a href="/product/detail/${product.productId}">
                        <img src="${img}" alt="${product.name}" class="search-result-img">
                        <div class="search-result-info">
                            <span class="search-result-name">${product.name}</span>
                            <span class="search-result-price">${price}</span>
                        </div>
                    </a>
                </li>`;
        });


        searchResults.html(html).show();
    }

    function displayNoResult(query, message = null) {
        const displayMessage = message ? message : `Không tìm thấy sản phẩm nào phù hợp với từ khóa "<strong>${query}</strong>"`;
        const html = `
            <li class="search-info-li">
                <i class="fi fi-rs-search-alt"></i>
                <span>${displayMessage}</span>
            </li>`;
        searchResults.html(html).show();
    }

    $(document).on('click', function (e) {
        if (!$(e.target).closest(searchFormContainer).length) {
            searchResults.hide();
        }
    });
});