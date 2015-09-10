/**
 * Copyright 2015 Marc Rufer, d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

﻿using System;
using System.ComponentModel.DataAnnotations;

namespace biz.dfch.CS.Utilities.Contracts.Model
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(36)]
        [StringLength(36)]
        public String Tid { get; set; }

        [Required]
        [MaxLength(1024)]
        [StringLength(1024)]
        public String Name { get; set; }

        public String Description { get; set; }

        [Required]
        public String CreatedBy { get; set; }
        [Required]
        public String ModifiedBy { get; set; }
        [Required]
        public DateTimeOffset Created { get; set; }
        [Required]
        public DateTimeOffset Modified { get; set; }
    }
}
