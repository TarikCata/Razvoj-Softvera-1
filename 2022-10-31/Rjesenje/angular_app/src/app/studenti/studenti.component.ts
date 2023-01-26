import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { MojConfig } from '../moj-config';
import { Router } from '@angular/router';

declare function porukaSuccess(a: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css'],
})
export class StudentiComponent implements OnInit {
  title: string = 'angularFIT2';
  ime_prezime: string = '';
  opstina: string = '';
  studentData: any;
  filter_ime_prezime: boolean;
  filter_opstina: boolean;
  dialogData: any;
  isDialogOpen: boolean = false;

  constructor(private httpKlijent: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.testirajWebApi();
  }

  testirajWebApi(params?: HttpParams): void {
    const url = `${MojConfig.adresa_servera}/Student/GetAll`;
    this.httpKlijent
      .get(url, { params })
      .subscribe((res) => (this.studentData = res));
  }

  filterStudents = () => {
    const params = new HttpParams()
      .append('ime_prezime', this.ime_prezime)
      .append('opstina', this.opstina);
    this.testirajWebApi(params);
  };

  openDialog = (student?: any) => {
    this.isDialogOpen = true;
    if (!!student) {
      const { ime, prezime, id, broj_indeksa, opstina_rodjenja } = student;
      this.dialogData = {
        title: 'Edit Student',
        studentData: { id, ime, prezime, broj_indeksa, opstina_rodjenja },
      };
      return;
    }
    this.dialogData = {
      title: 'Novi Student',
      studentData: {
        id: 0,
        ime: !!this.ime_prezime ? this.ime_prezime : '',
        prezime: '',
        broj_indeksa: '',
      },
    };
  };

  closeDialog = () => (this.isDialogOpen = false);

  redirect = (id: number) =>
    this.router.navigate(['student-maticnaknjiga', id]);
}
