import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/Models/UserModel';
import { Appointment } from 'src/app/Models/AppointmentModel';
import { UserService } from 'src/app/Services/user.service';
import { AppointmentService } from 'src/app/Services/appointment.service';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User;
  appointments: Appointment[];
  constructor(public userService: UserService, private appService: AppointmentService) {}

  ngOnInit(): void {
    this.userService.isFetching = true;
    this.userService.myAccount().subscribe(user => {
      this.user = user;
      if(parseInt(this.user.cnp.substr(0,1)) == 1 || parseInt(this.user.cnp.substr(0,1)) == 5){
        this.user.gender = "Male";
      }else {
        this.user.gender = "Female";
      }
      if(user.isApproved == 1) {
        this.userService.isApproved = true;
        this.userService.isFetching = false;
      }
    });

    this.appService.getPastAppointments().subscribe(appointments => {
      this.userService.isFetching = false;
        this.appointments = appointments;
    });
  }
}
