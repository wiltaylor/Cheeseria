import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-cheese',
  templateUrl: './cheese.component.html',
  styleUrls: ['./cheese.component.css']
})
export class CheeseComponent implements OnInit {

  constructor(private route: ActivatedRoute) { }

  Id: number;

  ngOnInit(): void {
    this.Id = parseInt(this.route.snapshot.paramMap.get('id'));
  }

}
