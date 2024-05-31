PORTALAPP.controller('MasterController', function ($scope, $http) {
 
});






function fullCalendar() {

	/* initialize the external events
		-----------------------------------------------------------------*/

	var containerEl = document.getElementById('external-events');
	if ($('#external-events').length > 0) {
		new FullCalendar.Draggable(containerEl, {
			itemSelector: '.external-event',
			eventData: function (eventEl) {
				return {
					title: eventEl.innerText.trim()
				}
			}

		});
	}
	/* initialize the calendar
	-----------------------------------------------------------------*/

	var calendarEl = document.getElementById('calendar');
	var calendar = new FullCalendar.Calendar(calendarEl, {

		eventClick: function (info) {
			//alert(JSON.stringify(info));
			//alert('Event: ' + info.event.title);
			alert('ID: ' + info.event.groupId);
			//alert('Coordinates: ' + info.jsEvent.pageX + ',' + info.jsEvent.pageY);
			//alert('View: ' + info.view.type);

			// change the border color just for fun
			//info.el.style.borderColor = 'red';
		},
		mouseLeave: function (info) {
			// change the border color just for fun
			info.el.style.borderColor = 'green';
		},
		
		headerToolbar: {
			left: 'prev,next today',
			center: 'title',
			right: 'dayGridMonth,timeGridDay'//,timeGridWeek,timeGridDay'
		},

		selectable: true,
		selectMirror: true,
		select: function (arg) {

			var date = arg.start;
			//alert(arg.start);
			/*
			
			
			alert(date.getDate());
			alert(date.getMonth());
			alert(date.getFullYear());
			*/
			$("#modalEventTitle").html("Novo evento : "+  date.getDate() + "/" + date.getMonth() + "/" + date.getFullYear())
			$("#modalEditEvent").modal("show");
		//	/*var title = prompt('Event Title:');
		//	if (title) {
		//		calendar.addEvent({
		//			title: title,
		//			start: arg.start,
		//			end: arg.end,
		//			allDay: arg.allDay
		//		})
		//	}
		//	*/
			calendar.unselect()
		},

		//editable: false,
		droppable: false, // this allows things to be dropped onto the calendar
		drop: function (arg) {
			// is the "remove after drop" checkbox checked?
			if (document.getElementById('drop-remove').checked) {
				// if so, remove the element from the "Draggable Events" list
				arg.draggedEl.parentNode.removeChild(arg.draggedEl);
			}
		},
		initialDate: '2024-05-31',
		weekNumbers: false,
		navLinks: false, // can click day/week names to navigate views
		editable: false,
		selectable: true,
		nowIndicator: true,
		events: [
			//{
			//	title: 'All Day Event',
			//	start: '2021-02-01'
			//},
			//{
			//	title: 'Long Event',
			//	start: '2021-02-07',
			//	end: '2021-02-10',
			//	className: "bg-danger"
			//},
			{
				//groupId: 999,
				groupId: 1,
				code: 1001,
				title: 'Repeating Event 1',
				start: '2024-05-31T16:00:00'
			},
			{
				//groupId: 999,
				groupId: 2,
				title: 'Repeating Event 2',
				start: '2024-05-31T16:00:00'
			},
			//{
			//	title: 'Conference',
			//	start: '2021-02-11',
			//	end: '2021-02-13',
			//	className: "bg-danger"
			//},
			{
				groupId: 3,
				title: 'Lunch',
				start: '2024-05-31T12:00:00'
			},
			{
				groupId: 4,
				title: 'Meeting',
				start: '2024-05-31T14:30:00'
			},
			{
				groupId: 5,
				title: 'Happy Hour',
				start: '2024-05-31T17:30:00'
			},
			//{	
			//	title: 'Dinner',
			//	start: '2024-05-31T20:00:00',
			//	className: "bg-warning"
			//},
			{
				groupId: 6,
				title: 'Birthday Party',
				start: '2024-05-31T07:00:00',
				className: "bg-secondary"
			},
			//{
			//	title: 'Click for Google xx',
			//	url: 'http://google.com/',
			//	start: '2024-05-31'
			//}
		]
	});
	calendar.render();
	calendar.setOption('locale', 'pt-BR');
}



jQuery(window).on('load', function () {
	setTimeout(function () {
		fullCalendar();
	}, 1000);


});




