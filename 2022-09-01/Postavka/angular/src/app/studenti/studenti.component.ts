import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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
  filter_imeprezime: string = '';
  filter_opstina: string = '';
  studentPodaci: any;
  checked_opstina: boolean;
  checked_imeprezime: boolean;
  constructor(private httpKlijent: HttpClient, private router: Router) {}

  testirajWebApi(): void {
    this.httpKlijent
      .get(
        MojConfig.adresa_servera + '/Student/GetAll',
        MojConfig.http_opcije()
      )
      .subscribe((x: any) => {
        this.studentPodaci = x;
      });
  }

  ngOnInit(): void {
    this.testirajWebApi();
  }
}
