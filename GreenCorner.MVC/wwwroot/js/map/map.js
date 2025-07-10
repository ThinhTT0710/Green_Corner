let map, marker, streetView;
const locationStatus = document.getElementById('locationStatus');
const locationInput = document.getElementById('locationInput');

function showTab(tabName) {
    const mapContainer = document.getElementById('mapContainer');
    const streetContainer = document.getElementById('streetContainer');
    const btnMap = document.getElementById('btnMap');
    const btnStreet = document.getElementById('btnStreet');

    if (tabName === 'map') {
        mapContainer.style.display = 'block';
        streetContainer.style.display = 'none';
        btnMap.className = 'btn btn-primary';
        btnStreet.className = 'btn btn-outline-secondary';
    } else {
        mapContainer.style.display = 'none';
        streetContainer.style.display = 'block';
        btnMap.className = 'btn btn-outline-secondary';
        btnStreet.className = 'btn btn-primary';
    }
}

function setLocationStatus(message, type) {
    locationStatus.innerHTML = message;
    locationStatus.className = `alert alert-${type}`;
}

function getLocation() {
    if (navigator.geolocation) {
        setLocationStatus('🛰️ Đang dò tìm vị trí của bạn, vui lòng chờ...', 'info');

        navigator.geolocation.getCurrentPosition(
            position => {
                const lat = roundCoord(position.coords.latitude);
                const lng = roundCoord(position.coords.longitude);
                const accuracy = position.coords.accuracy;

                if (accuracy > 100) {
                    setLocationStatus(`⚠️ Độ chính xác vị trí là ±${Math.round(accuracy)}m. <b>Hãy kéo ghim trên bản đồ</b> để có vị trí đúng nhất.`, 'warning');
                } else {
                    setLocationStatus(`✅ Lấy vị trí thành công (độ chính xác ±${Math.round(accuracy)}m).`, 'success');
                }

                updateLatLng(lat, lng);
                if (!map) {
                    initMap(lat, lng);
                } else {
                    const newPos = new google.maps.LatLng(lat, lng);
                    map.setCenter(newPos);
                    marker.setPosition(newPos);
                }
            },
            err => {
                setLocationStatus(`🚫 Lỗi khi lấy vị trí: ${err.message}`, 'danger');
            },
            { enableHighAccuracy: true, timeout: 10000, maximumAge: 0 }
        );
    } else {
        setLocationStatus('Trình duyệt của bạn không hỗ trợ định vị.', 'danger');
    }
}

function initMap(lat, lng) {
    const position = { lat, lng };
    map = new google.maps.Map(document.getElementById("mapContainer"), {
        center: position,
        zoom: 17,
        mapTypeControl: false,
        streetViewControl: false,
    });

    marker = new google.maps.Marker({
        position: position,
        map: map,
        draggable: true,
        animation: google.maps.Animation.DROP,
        title: "Kéo để chọn vị trí chính xác"
    });

    marker.addListener('dragend', e => {
        const lat = roundCoord(e.latLng.lat());
        const lng = roundCoord(e.latLng.lng());
        updateLatLng(lat, lng);
        setLocationStatus('👍 Đã cập nhật vị trí thủ công.', 'success');
    });

    map.addListener('click', e => {
        const lat = roundCoord(e.latLng.lat());
        const lng = roundCoord(e.latLng.lng());
        marker.setPosition({ lat, lng });
        updateLatLng(lat, lng);
        setLocationStatus('👍 Đã cập nhật vị trí thủ công.', 'success');
    });

    initStreetView(lat, lng);
}

function updateLatLng(lat, lng) {
    locationInput.value = `${lat}, ${lng}`;
    initStreetView(lat, lng);
}

// ĐÃ BỎ HÀM getAddressFromCoordinates

function initStreetView(lat, lng) {
    const svService = new google.maps.StreetViewService();
    svService.getPanorama({ location: { lat, lng }, radius: 50 }, (data, status) => {
        const streetContainer = document.getElementById("streetContainer");
        if (status === "OK") {
            streetView = new google.maps.StreetViewPanorama(streetContainer, {
                position: data.location.latLng,
                pov: { heading: 165, pitch: 0 },
                visible: true
            });
            map.setStreetView(streetView);
        } else {
            streetContainer.innerHTML = `<div class='alert alert-light text-center mt-5'>Không có chế độ Street View tại vị trí này.</div>`;
        }
    });
}

function roundCoord(coord) {
    return Math.round(coord * 1e6) / 1e6;
}

function setInitialLocation(lat, lng) {
    updateLatLng(lat, lng);
    initMap(lat, lng);
}