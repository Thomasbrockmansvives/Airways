document.addEventListener('DOMContentLoaded', function () {
    const flightSearchForm = document.getElementById('flightSearchForm');
    const departureCitySelect = document.getElementById('departureCity');
    const arrivalCitySelect = document.getElementById('arrivalCity');
    const startDateInput = document.getElementById('startDate');
    const endDateInput = document.getElementById('endDate');

    

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

          
        });
    }
});