document.addEventListener('DOMContentLoaded', function () {
    const flightSearchForm = document.getElementById('flightSearchForm');
    const departureCitySelect = document.getElementById('departureCity');
    const arrivalCitySelect = document.getElementById('arrivalCity');
    const startDateInput = document.getElementById('startDate');
    const endDateInput = document.getElementById('endDate');

    if (startDateInput && endDateInput) {
        startDateInput.addEventListener('change', function () {
            if (endDateInput.value && new Date(endDateInput.value) < new Date(startDateInput.value)) {
                endDateInput.value = startDateInput.value;
            }
            endDateInput.min = startDateInput.value;
        });
    }

    if (flightSearchForm) {
        flightSearchForm.addEventListener('submit', function (event) {
            if (departureCitySelect && arrivalCitySelect) {
                const departureCity = departureCitySelect.value;
                const arrivalCity = arrivalCitySelect.value;

                if (departureCity === arrivalCity && departureCity !== '') {
                    alert('Departure and arrival cities cannot be the same.');
                    event.preventDefault();
                    return false;
                }
            }

            if (startDateInput && endDateInput) {
                const startDate = new Date(startDateInput.value);
                const endDate = new Date(endDateInput.value);

                if (endDate < startDate) {
                    alert('End date cannot be before start date.');
                    event.preventDefault();
                    return false;
                }
            }
        });
    }
});