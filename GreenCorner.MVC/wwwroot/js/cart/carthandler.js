$(document).ready(function () {
    function updateCartQuantity(cartId, productId, newQuantity) {
        if (isNaN(newQuantity) || newQuantity < 1) {
            Swal.fire({
                icon: 'warning',
                title: 'Số lượng không hợp lệ!',
                text: 'Vui lòng nhập số lượng lớn hơn hoặc bằng 1.',
            });
            return;
        }

        $.ajax({
            url: '/Cart/UpdateQuantity',
            type: 'POST',
            data: {
                cartId: cartId,
                productId: productId,
                quantity: newQuantity
            },
            success: function (response) {
                if (response.isSuccess) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công!',
                        text: response.message,
                        showConfirmButton: false,
                        timer: 1500
                    }).then(() => {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi!',
                        text: response.message || 'Có lỗi xảy ra khi cập nhật số lượng.',
                    });
                    var $targetInput = $(`.qty-val[data-cart-id="${cartId}"]`);
                    var initialQuantity = parseInt($targetInput.attr('value'));
                    if (!isNaN(initialQuantity)) {
                        $targetInput.val(initialQuantity);
                    }
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi!',
                    text: 'Không thể kết nối đến máy chủ để cập nhật số lượng.',
                });
                var $targetInput = $(`.qty-val[data-cart-id="${cartId}"]`);
                var initialQuantity = parseInt($targetInput.attr('value'));
                if (!isNaN(initialQuantity)) {
                    $targetInput.val(initialQuantity);
                }
            }
        });
    }

    $('.qty-up').off('click').on('click', function (e) {
        e.preventDefault();
        var $qtyInput = $(this).siblings('.qty-val');
        var currentVal = parseInt($qtyInput.val());
        var cartId = $(this).data('cart-id');
        var productId = $(this).data('product-id');

        if (!isNaN(currentVal)) {
            var newQuantity = currentVal + 1;
            $qtyInput.val(newQuantity);
            updateCartQuantity(cartId, productId, newQuantity);
        }
    });

    $('.qty-down').off('click').on('click', function (e) {
        e.preventDefault();
        var $qtyInput = $(this).siblings('.qty-val');
        var currentVal = parseInt($qtyInput.val());
        var cartId = $(this).data('cart-id');
        var productId = $(this).data('product-id');

        if (!isNaN(currentVal) && currentVal > 1) {
            var newQuantity = currentVal - 1;
            $qtyInput.val(newQuantity);
            updateCartQuantity(cartId, productId, newQuantity);
        } else if (currentVal === 1) {
            Swal.fire({
                icon: 'warning',
                title: 'Không thể giảm thêm!',
                text: 'Số lượng sản phẩm không thể nhỏ hơn 1. Bạn có thể xóa sản phẩm nếu không muốn mua nữa.',
            });
        }
    });

    $('.qty-val').off('change').on('change', function () {
        var newQuantity = parseInt($(this).val());
        var cartId = $(this).data('cart-id');
        var productId = $(this).data('product-id');

        updateCartQuantity(cartId, productId, newQuantity);
    });

    $('.remove-cart-item').off('click').on('click', function (e) {
        e.preventDefault();
        var cartId = $(this).data('cart-id');

        Swal.fire({
            title: 'Bạn có chắc chắn muốn xóa?',
            text: "Sản phẩm này sẽ bị xóa khỏi giỏ hàng của bạn!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có, xóa nó đi!',
            cancelButtonText: 'Không, hủy bỏ!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Cart/Remove',
                    type: 'GET',
                    data: { cartId: cartId },
                    success: function (response) {
                        if (response.isSuccess) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Đã xóa!',
                                text: response.message,
                                showConfirmButton: false,
                                timer: 1500
                            }).then(() => {
                                location.reload();
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Lỗi!',
                                text: response.message || 'Có lỗi xảy ra khi xóa sản phẩm.',
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi!',
                            text: 'Không thể kết nối đến máy chủ để xóa sản phẩm.',
                        });
                    }
                });
            }
        });
    });
});

$(document).ready(function () {
    $('.add-to-cart-btn').off('click').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('product-id');

        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                Swal.fire({
                    icon: response.icon,
                    title: response.isSuccess ? 'Thành công!' : 'Lỗi!',
                    text: response.message,
                    showConfirmButton: true,
                    timer: response.isSuccess ? 1500 : undefined
                }).then(() => {
                    if (response.isSuccess) {
                    } else if (response.message === "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng.") {
                        window.location.href = "/Auth/Login";
                    }
                });
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi!',
                    text: 'Không thể thêm sản phẩm vào giỏ hàng. Vui lòng thử lại sau.',
                });
            }
        });
    });
});

$(document).ready(function () {
    function addToCartAjax(productId, quantity) {
        if (isNaN(quantity) || quantity < 1) {
            Swal.fire({
                icon: 'warning',
                title: 'Số lượng không hợp lệ!',
                text: 'Vui lòng nhập số lượng lớn hơn hoặc bằng 1.',
            });
            return;
        }

        $.ajax({
            url: '/Cart/AddToCartWithQuantity',
            type: 'POST',
            data: { productId: productId, quantity: quantity },
            success: function (response) {
                Swal.fire({
                    icon: response.icon,
                    title: response.isSuccess ? 'Thành công!' : 'Lỗi!',
                    text: response.message,
                    showConfirmButton: true,
                    timer: response.isSuccess ? 1500 : undefined
                }).then(() => {
                    if (!response.isSuccess && response.message === "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng.") {
                        window.location.href = "/Identity/Account/Login";
                    }
                });
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi!',
                    text: 'Không thể thêm sản phẩm vào giỏ hàng. Vui lòng thử lại sau.',
                });
            }
        });
    }

    $('.qty-up-detail').off('click').on('click', function (e) {
        e.preventDefault();
        var $qtyInput = $(this).siblings('.qty-val-detail');
        var currentVal = parseInt($qtyInput.val());
        if (!isNaN(currentVal)) {
            $qtyInput.val(currentVal + 1);
        }
    });

    $('.qty-down-detail').off('click').on('click', function (e) {
        e.preventDefault();
        var $qtyInput = $(this).siblings('.qty-val-detail');
        var currentVal = parseInt($qtyInput.val());
        if (!isNaN(currentVal) && currentVal > 1) {
            $qtyInput.val(currentVal - 1);
        }
    });

    $('.qty-val-detail').off('change').on('change', function () {
        var newQuantity = parseInt($(this).val());
        if (isNaN(newQuantity) || newQuantity < 1) {
            Swal.fire({
                icon: 'warning',
                title: 'Số lượng không hợp lệ!',
                text: 'Vui lòng nhập số lượng lớn hơn hoặc bằng 1.',
            });
            $(this).val(1);
        }
    });

    $('.button-add-to-cart-detail').off('click').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('product-id');
        var quantity = parseInt($('.qty-val-detail').val());
        addToCartAjax(productId, quantity);
    });
});

    $(document).ready(function () {
        $('.remove-wishlist-item').off('click').on('click', function (e) {
            e.preventDefault();
            var wishListId = $(this).data('wishlist-id');

            Swal.fire({
                title: 'Bạn có chắc chắn muốn xóa?',
                text: "Sản phẩm này sẽ bị xóa khỏi danh sách yêu thích của bạn!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Có, xóa nó đi!',
                cancelButtonText: 'Không, hủy bỏ!'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/WishList/RemoveItem',
                        type: 'GET',
                        data: { wishListId: wishListId },
                        success: function (response) {
                            if (response.isSuccess) {
                                Swal.fire({
                                    icon: 'success',
                                    title: 'Đã xóa!',
                                    text: response.message,
                                    showConfirmButton: false,
                                    timer: 1500
                                }).then(() => {
                                    location.reload();
                                });
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Lỗi!',
                                    text: response.message || 'Có lỗi xảy ra khi xóa sản phẩm.',
                                });
                            }
                        },
                        error: function (xhr, status, error) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Lỗi!',
                                text: 'Không thể kết nối đến máy chủ để xóa sản phẩm.',
                            });
                        }
                    });
                }
            });
        });
    });

$(document).ready(function () {
    $(document).on('click', '.wishlist-toggle-btn', function (e) {
        e.preventDefault();

        var $button = $(this);
        var productId = $button.data('product-id');

        var isWishlisted = $button.hasClass('active');

        var url = isWishlisted ? '/WishList/Remove' : '/WishList/AddToWishList';

        $.ajax({
            url: url,
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                if (response.isSuccess) {
                    $button.toggleClass('active');

                    if ($button.hasClass('active')) {
                        $button.attr('aria-label', 'Remove from Wishlist');
                    } else {
                        $button.attr('aria-label', 'Add To Wishlist');
                    }

                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công!',
                        text: response.message,
                        showConfirmButton: false,
                        timer: 1500
                    });

                } else {
                    if (response.message.includes("đăng nhập")) {
                        Swal.fire({
                            icon: 'warning',
                            title: 'Yêu cầu đăng nhập',
                            text: response.message,
                            confirmButtonText: 'Đăng nhập ngay'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = "/Auth/Login";
                            }
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi!',
                            text: response.message,
                        });
                    }
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi kết nối!',
                    text: 'Không thể kết nối đến máy chủ. Vui lòng thử lại sau.',
                });
            }
        });
    });

    $('.remove-wishlist-item').off('click').on('click', function (e) {
        e.preventDefault();
        var wishListId = $(this).data('wishlist-id');

        Swal.fire({
            title: 'Bạn có chắc chắn muốn xóa?',
            text: "Sản phẩm này sẽ bị xóa khỏi danh sách yêu thích của bạn!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có, xóa nó đi!',
            cancelButtonText: 'Không, hủy bỏ!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/WishList/RemoveItem',
                    type: 'GET',
                    data: { wishListId: wishListId },
                    success: function (response) {
                        if (response.isSuccess) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Đã xóa!',
                                text: response.message,
                                showConfirmButton: false,
                                timer: 1500
                            }).then(() => {
                                location.reload();
                            });
                        } else {
                        }
                    },
                    error: function () {
                    }
                });
            }
        });
    });
});